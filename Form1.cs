using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace NetworkScannerApp
{
    public partial class MainForm : Form
    {
        private BackgroundWorker scanWorker;
        private CancellationTokenSource cancellationTokenSource;
        private List<NetworkDevice> scanResults;
        private NetworkInfo networkInfo;
        private Stopwatch scanStopwatch = new Stopwatch();
        private int totalIPs;
        private int scannedIPs;
        private const int ClosedPortThreshold = 90; // Percentage of closed ports to trigger firewall warning

        public MainForm()
        {
            InitializeComponent();
            InitializeScanWorker();
            progressBarScan.Style = ProgressBarStyle.Continuous;
            progressBarScan.Minimum = 0;
            progressBarScan.Maximum = 100;
            progressBarScan.Value = 0;
            textBoxLog.ReadOnly = true;
            textBoxTimeout.Text = "1000"; // Default TCP timeout
            textBoxUdpTimeout.Text = "500"; // Default UDP timeout
            comboBoxScanMode.Items.AddRange(new[] { "Quét Nhanh", "Quét Đầy Đủ", "Quét Tùy Chỉnh" });
            comboBoxScanMode.SelectedIndex = 0; // Default to Quick Scan
            UpdatePortTextBoxes();
        }

        private void InitializeScanWorker()
        {
            scanWorker = new BackgroundWorker();
            scanWorker.WorkerReportsProgress = true;
            scanWorker.WorkerSupportsCancellation = true;
            scanWorker.DoWork += ScanWorker_DoWork;
            scanWorker.ProgressChanged += ScanWorker_ProgressChanged;
            scanWorker.RunWorkerCompleted += ScanWorker_RunWorkerCompleted;
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (scanWorker.IsBusy)
            {
                labelScanInfo.Text = $"Thông tin quét: Đang quét {scannedIPs}/{totalIPs} IP, Tìm thấy {scanResults.Count} thiết bị, Thời gian: {scanStopwatch.Elapsed:mm\\:ss}";
            }
        }

        private void comboBoxScanMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePortTextBoxes();
        }

        private void UpdatePortTextBoxes()
        {
            string selectedMode = comboBoxScanMode.SelectedItem?.ToString();
            if (selectedMode == "Quét Nhanh")
            {
                textBoxTcpPortRange.Text = "21,22,23,80,110,143,443,445,3389,8080";
                textBoxUdpPortRange.Text = "53,67,68,123,161,500,5060";
                textBoxTcpPortRange.Enabled = false;
                textBoxUdpPortRange.Enabled = false;
            }
            else if (selectedMode == "Quét Đầy Đủ")
            {
                textBoxTcpPortRange.Text = "1-65535";
                textBoxUdpPortRange.Text = "1-65535";
                textBoxTcpPortRange.Enabled = false;
                textBoxUdpPortRange.Enabled = false;
            }
            else // Quét Tùy Chỉnh
            {
                textBoxTcpPortRange.Text = "1-1000";
                textBoxUdpPortRange.Text = "1-1000";
                textBoxTcpPortRange.Enabled = true;
                textBoxUdpPortRange.Enabled = true;
            }
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            if (scanWorker.IsBusy)
            {
                MessageBox.Show("Quá trình quét đang được thực hiện.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxTimeout.Text, out int tcpTimeout) || tcpTimeout < 100 || tcpTimeout > 10000)
            {
                MessageBox.Show("Vui lòng nhập thời gian chờ TCP từ 100 đến 10000 ms.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBoxUdpTimeout.Text, out int udpTimeout) || udpTimeout < 100 || udpTimeout > 10000)
            {
                MessageBox.Show("Vui lòng nhập thời gian chờ UDP từ 100 đến 10000 ms.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            networkInfo = GetNetworkInfo();
            if (networkInfo == null)
            {
                MessageBox.Show("Không thể xác định cấu hình mạng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisplayNetworkInfo(networkInfo);
            scanResults = new List<NetworkDevice>();
            listViewResults.Items.Clear();
            textBoxLog.Clear();
            cancellationTokenSource = new CancellationTokenSource();
            scanStopwatch.Restart();
            scannedIPs = 0;
            totalIPs = networkInfo.TotalIPs;
            progressBarScan.Value = 0;
            statusLabel.Text = "Đang quét...";
            labelScanInfo.Text = $"Thông tin quét: Đang quét 0/{totalIPs} IP";
            toolStripButtonStart.Enabled = false;
            toolStripButtonStop.Enabled = true;
            toolStripDropDownButtonExport.Enabled = false;
            toolStripButtonSaveLog.Enabled = false;
            timerUpdate.Start();
            AppendLog($"Bắt đầu {comboBoxScanMode.SelectedItem} với cổng TCP ({textBoxTcpPortRange.Text}), cổng UDP ({textBoxUdpPortRange.Text})");
            scanWorker.RunWorkerAsync();
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            if (scanWorker.IsBusy)
            {
                cancellationTokenSource.Cancel();
                statusLabel.Text = "Đang hủy...";
                toolStripButtonStop.Enabled = false;
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
            AppendLog("Nhật ký đã được xóa.");
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLog.Text))
            {
                MessageBox.Show("Không có nội dung nhật ký để lưu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Log Files (*.log)|*.log",
                FileName = $"network_scan_log_{DateTime.Now:yyyyMMdd_HHmmss}.log"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                File.WriteAllText(path, textBoxLog.Text);
                MessageBox.Show($"Nhật ký đã được lưu vào {path}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportTxt_Click(object sender, EventArgs e)
        {
            ExportResults(".txt");
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            ExportResults(".csv");
        }

        private void ScanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var devices = ScanNetwork(networkInfo, cancellationTokenSource.Token, scanWorker);
            e.Result = devices;
        }

        private void ScanWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var device = e.UserState as NetworkDevice;
            if (device != null)
            {
                var item = new ListViewItem(new[] {
                    device.IPAddress,
                    device.MacAddress,
                    device.HostName,
                    string.Join(", ", device.OpenPorts.Select(p => $"{p.PortNumber}({p.Service})"))
                });
                listViewResults.Items.Add(item);
                scanResults.Add(device);
            }
            progressBarScan.Value = Math.Min(e.ProgressPercentage, 100);
        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            scanStopwatch.Stop();
            timerUpdate.Stop();
            progressBarScan.Value = 0;
            toolStripButtonStart.Enabled = true;
            toolStripButtonStop.Enabled = false;
            toolStripDropDownButtonExport.Enabled = true;
            toolStripButtonSaveLog.Enabled = true;

            if (e.Cancelled)
            {
                statusLabel.Text = "Quét đã bị hủy";
                AppendLog("Quét đã bị hủy.");
                MessageBox.Show("Quét đã bị hủy.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                statusLabel.Text = "Quét thất bại";
                AppendLog($"Quét thất bại: {e.Error.Message}");
                MessageBox.Show($"Quét thất bại: {e.Error.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                scanResults = e.Result as List<NetworkDevice>;
                statusLabel.Text = $"Quét hoàn tất. Tìm thấy {scanResults.Count} thiết bị.";
                AppendLog($"Quét hoàn tất trong {scanStopwatch.Elapsed}. Tìm thấy {scanResults.Count} thiết bị.");
                MessageBox.Show($"Quét hoàn tất trong {scanStopwatch.Elapsed}. Tìm thấy {scanResults.Count} thiết bị.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            labelScanInfo.Text = $"Thông tin quét: Đã quét {scannedIPs}/{totalIPs} IP, Tìm thấy {scanResults.Count} thiết bị, Thời gian: {scanStopwatch.Elapsed:mm\\:ss}";
        }

        private void AppendLog(string message)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new Action(() => textBoxLog.AppendText($"{message}\r\n")));
            }
            else
            {
                textBoxLog.AppendText($"{message}\r\n");
            }
        }

        private NetworkInfo GetNetworkInfo()
        {
            var localIP = GetLocalIPAddress();
            if (localIP == null) return null;

            var subnetMask = GetSubnetMask(localIP);
            if (subnetMask == null) return null;

            var networkAddress = CalculateNetworkAddress(localIP, subnetMask);
            var broadcastAddress = CalculateBroadcastAddress(localIP, subnetMask);
            var cidr = CalculateCIDR(subnetMask);

            return new NetworkInfo
            {
                LocalIP = localIP,
                SubnetMask = subnetMask,
                NetworkAddress = networkAddress,
                BroadcastAddress = broadcastAddress,
                FirstIP = GetFirstHostIP(networkAddress),
                LastIP = GetLastHostIP(broadcastAddress),
                CIDR = cidr,
                TotalIPs = (int)Math.Pow(2, 32 - cidr) - 2
            };
        }

        private void DisplayNetworkInfo(NetworkInfo info)
        {
            labelNetworkInfo.Text = $"Địa chỉ IP cục bộ: {info.LocalIP}\n" +
                                    $"Subnet Mask: {info.SubnetMask}\n" +
                                    $"Địa chỉ mạng: {info.NetworkAddress}/{info.CIDR}\n" +
                                    $"Phạm vi IP: {info.FirstIP} - {info.LastIP}";
        }

        private List<NetworkDevice> ScanNetwork(NetworkInfo info, CancellationToken token, BackgroundWorker worker)
        {
            var activeDevices = new List<NetworkDevice>();
            uint startIP = BitConverter.ToUInt32(info.FirstIP.GetAddressBytes().Reverse().ToArray(), 0);
            uint endIP = BitConverter.ToUInt32(info.LastIP.GetAddressBytes().Reverse().ToArray(), 0);
            totalIPs = (int)(endIP - startIP + 1);
            scannedIPs = 0;

            var (tcpPortStart, tcpPortEnd) = GetPortRange(textBoxTcpPortRange.Text);
            var (udpPortStart, udpPortEnd) = GetPortRange(textBoxUdpPortRange.Text);
            int totalPorts = (tcpPortEnd - tcpPortStart + 1) + (udpPortEnd - udpPortStart + 1);
            int semaphoreLimit = totalPorts > 1000 ? 50 : 100;
            var semaphore = new SemaphoreSlim(semaphoreLimit);
            var tasks = new List<Task>();

            for (uint ip = startIP; ip <= endIP; ip++)
            {
                if (token.IsCancellationRequested) break;

                var ipBytes = BitConverter.GetBytes(ip).Reverse().ToArray();
                var ipAddress = new IPAddress(ipBytes);

                semaphore.Wait(token);
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var device = await ScanDeviceAsync(ipAddress, token);
                        lock (activeDevices)
                        {
                            scannedIPs++;
                            int progress = (int)((double)scannedIPs / totalIPs * 100);
                            if (device != null)
                            {
                                activeDevices.Add(device);
                                worker.ReportProgress(progress, device);
                            }
                            else
                            {
                                if (scannedIPs % 10 == 0)
                                {
                                    worker.ReportProgress(progress, null);
                                }
                            }
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, token));
            }

            Task.WaitAll(tasks.ToArray());
            return activeDevices.OrderBy(d => d.IPAddress).ToList();
        }

        private async Task<NetworkDevice> ScanDeviceAsync(IPAddress ipAddress, CancellationToken token)
        {
            bool isAlive = await ArpPing(ipAddress) || await IcmpPing(ipAddress, token);
            if (!isAlive) return null;

            var macAddress = await GetMacAddressAsync(ipAddress);
            var hostName = await GetHostNameAsync(ipAddress);
            var (tcpPortStart, tcpPortEnd) = GetPortRange(textBoxTcpPortRange.Text);
            var (udpPortStart, udpPortEnd) = GetPortRange(textBoxUdpPortRange.Text);
            var tcpPorts = ParsePortList(textBoxTcpPortRange.Text, tcpPortStart, tcpPortEnd);
            var udpPorts = ParsePortList(textBoxUdpPortRange.Text, udpPortStart, udpPortEnd);
            int totalPorts = tcpPorts.Length + udpPorts.Length;

            int tcpTimeout, udpTimeout;
            if (!int.TryParse(textBoxTimeout.Text, out tcpTimeout) || tcpTimeout < 100 || tcpTimeout > 10000)
            {
                tcpTimeout = 1000; // Default TCP timeout
            }
            if (!int.TryParse(textBoxUdpTimeout.Text, out udpTimeout) || udpTimeout < 100 || udpTimeout > 10000)
            {
                udpTimeout = 500; // Default UDP timeout
            }

            var (tcpOpenPorts, tcpClosedPorts) = await ScanPortsAsync(ipAddress, tcpPorts, token, tcpTimeout);
            var (udpOpenPorts, udpClosedPorts) = await ScanUDPPortsAsync(ipAddress, udpPorts, token, udpTimeout);
            int closedPorts = tcpClosedPorts + udpClosedPorts;

            if (closedPorts * 100 / totalPorts >= ClosedPortThreshold)
            {
                AppendLog($"Cảnh báo: {closedPorts}/{totalPorts} cổng đóng trên {ipAddress}. Kiểm tra cài đặt tường lửa.");
            }

            return new NetworkDevice
            {
                IPAddress = ipAddress.ToString(),
                MacAddress = macAddress,
                HostName = hostName,
                OpenPorts = tcpOpenPorts.Concat(udpOpenPorts).ToList(),
                FirstSeen = DateTime.Now,
                LastSeen = DateTime.Now,
                Status = "Hoạt động"
            };
        }

        private async Task<(List<NetworkPort> openPorts, int closedPorts)> ScanPortsAsync(IPAddress ipAddress, int[] ports, CancellationToken token, int timeout)
        {
            var openPorts = new List<NetworkPort>();
            int closedPorts = 0;
            int consecutiveClosedPorts = 0;

            foreach (var port in ports)
            {
                for (int attempt = 0; attempt < 3; attempt++)
                {
                    using (var client = new TcpClient())
                    {
                        try
                        {
                            client.NoDelay = true;
                            var connectTask = client.ConnectAsync(ipAddress, port);
                            if (await Task.WhenAny(connectTask, Task.Delay(timeout, token)) == connectTask && client.Connected)
                            {
                                lock (openPorts)
                                {
                                    openPorts.Add(new NetworkPort { PortNumber = port, Service = GetServiceName(port) });
                                    AppendLog($"[{DateTime.Now}] Cổng TCP {port} mở trên {ipAddress}");
                                    consecutiveClosedPorts = 0;
                                }
                                break;
                            }
                            else
                            {
                                lock (openPorts)
                                {
                                    AppendLog($"[{DateTime.Now}] Cổng TCP {port} đóng hoặc hết thời gian chờ trên {ipAddress} (Lần thử {attempt + 1})");
                                    consecutiveClosedPorts++;
                                    closedPorts++;
                                }
                                await Task.Delay(500, token);
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (openPorts)
                            {
                                AppendLog($"[{DateTime.Now}] Quét cổng TCP {port} thất bại trên {ipAddress}: {ex.Message}");
                                consecutiveClosedPorts++;
                                closedPorts++;
                            }
                        }
                    }
                }
            }
            return (openPorts, closedPorts);
        }

        private async Task<(List<NetworkPort> openPorts, int closedPorts)> ScanUDPPortsAsync(IPAddress ipAddress, int[] udpPorts, CancellationToken token, int timeout)
        {
            var openPorts = new List<NetworkPort>();
            int closedPorts = 0;
            int consecutiveClosedPorts = 0;
            var semaphore = new SemaphoreSlim(50); // Limit to 50 concurrent UDP scans
            var tasks = new List<Task>();

            foreach (var port in udpPorts)
            {
                await semaphore.WaitAsync(token);
                tasks.Add(Task.Run(async () =>
                {
                    using (var client = new UdpClient())
                    {
                        try
                        {
                            byte[] data = GetUdpPacketForPort(port);
                            await client.SendAsync(data, data.Length, ipAddress.ToString(), port);
                            client.Client.ReceiveTimeout = timeout;
                            var receiveTask = client.ReceiveAsync();
                            if (await Task.WhenAny(receiveTask, Task.Delay(timeout, token)) == receiveTask)
                            {
                                lock (openPorts)
                                {
                                    openPorts.Add(new NetworkPort { PortNumber = port, Service = GetServiceName(port) });
                                    AppendLog($"[{DateTime.Now}] Cổng UDP {port} mở trên {ipAddress}");
                                    consecutiveClosedPorts = 0;
                                }
                            }
                            else
                            {
                                lock (openPorts)
                                {
                                    AppendLog($"[{DateTime.Now}] Cổng UDP {port} đóng hoặc hết thời gian chờ trên {ipAddress}");
                                    consecutiveClosedPorts++;
                                    closedPorts++;
                                }
                            }
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                        {
                            lock (openPorts)
                            {
                                AppendLog($"[{DateTime.Now}] Cổng UDP {port} đóng trên {ipAddress}: ICMP Port Unreachable");
                                consecutiveClosedPorts++;
                                closedPorts++;
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (openPorts)
                            {
                                AppendLog($"[{DateTime.Now}] Quét cổng UDP {port} thất bại trên {ipAddress}: {ex.Message}");
                                consecutiveClosedPorts++;
                                closedPorts++;
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }
                }, token));
            }

            await Task.WhenAll(tasks);
            semaphore.Dispose();
            return (openPorts, closedPorts);
        }

        private byte[] GetUdpPacketForPort(int port)
        {
            switch (port)
            {
                case 53: return CreateDnsQuery();
                case 161: return CreateSnmpQuery();
                case 67: case 68: return CreateDhcpDiscover();
                case 123: return CreateNtpRequest();
                case 500: return CreateIpsecPacket();
                case 5060: return CreateSipPacket();
                default: return Encoding.ASCII.GetBytes("ping");
            }
        }

        private byte[] CreateDnsQuery()
        {
            return new byte[] {
                0xAA, 0xAA, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x07, 0x65, 0x78, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x03, 0x63, 0x6F, 0x6D,
                0x00, 0x00, 0x01, 0x00, 0x01
            };
        }

        private byte[] CreateSnmpQuery()
        {
            return new byte[] {
                0x30, 0x26, 0x02, 0x01, 0x01, 0x04, 0x06, 0x70, 0x75, 0x62, 0x6C, 0x69,
                0x63, 0xA0, 0x1D, 0x02, 0x04, 0x01, 0x02, 0x03, 0x04, 0x02, 0x01, 0x00,
                0x02, 0x01, 0x00, 0x30, 0x10, 0x30, 0x0E, 0x06, 0x08, 0x2B, 0x06, 0x01,
                0x02, 0x01, 0x01, 0x01, 0x00, 0x05, 0x00
            };
        }

        private byte[] CreateDhcpDiscover()
        {
            return new byte[] {
                0x01, 0x01, 0x06, 0x00, 0x01, 0x02, 0x03, 0x04, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x35, 0x01,
                0x01, 0xFF
            };
        }

        private byte[] CreateNtpRequest()
        {
            return new byte[] {
                0x1B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
        }

        private byte[] CreateIpsecPacket()
        {
            return new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x05, 0x01, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x1C
            };
        }

        private byte[] CreateSipPacket()
        {
            string sipRequest = "OPTIONS sip:example.com SIP/2.0\r\n\r\n";
            return Encoding.ASCII.GetBytes(sipRequest);
        }

        private int[] ParsePortList(string input, int start, int end)
        {
            try
            {
                if (input.Contains(","))
                {
                    return input.Split(',')
                        .Select(s => int.Parse(s.Trim()))
                        .Where(p => p >= 1 && p <= 65535)
                        .Distinct()
                        .OrderBy(p => p)
                        .ToArray();
                }
                return Enumerable.Range(start, end - start + 1).ToArray();
            }
            catch
            {
                return Enumerable.Range(1, 1000).ToArray();
            }
        }

        private (int start, int end) GetPortRange(string input)
        {
            try
            {
                if (!input.Contains(","))
                {
                    var parts = input.Split('-').Select(s => int.Parse(s.Trim())).ToArray();
                    if (parts.Length != 2 || parts[0] < 1 || parts[1] > 65535 || parts[0] > parts[1])
                        throw new Exception();
                    return (parts[0], parts[1]);
                }
                return (1, 1000);
            }
            catch
            {
                return (1, 1000);
            }
        }

        private void ExportResults(string format)
        {
            if (scanResults == null || scanResults.Count == 0)
            {
                MessageBox.Show("Không có kết quả quét để xuất.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = $"Log Files (*{format})|*{format}",
                FileName = $"network_scan_result_{DateTime.Now:yyyyMMdd_HHmmss}{format}"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                if (format == ".txt")
                    SaveToTextFile(path);
                else if (format == ".csv")
                    SaveToCsvFile(path);
                MessageBox.Show($"Kết quả đã được xuất vào {path}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveToTextFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("=== Kết Quả Quét Mạng ===");
                writer.WriteLine($"Ngày quét: {DateTime.Now}");
                writer.WriteLine($"Thời gian quét: {scanStopwatch.Elapsed}");
                writer.WriteLine($"Chế độ quét: {comboBoxScanMode.SelectedItem}");
                writer.WriteLine();
                writer.WriteLine("=== Thông Tin Mạng ===");
                writer.WriteLine($"Địa chỉ IP cục bộ: {networkInfo.LocalIP}");
                writer.WriteLine($"Subnet Mask: {networkInfo.SubnetMask}");
                writer.WriteLine($"Địa chỉ mạng: {networkInfo.NetworkAddress}/{networkInfo.CIDR}");
                writer.WriteLine($"Phạm vi IP: {networkInfo.FirstIP} - {networkInfo.LastIP}");
                writer.WriteLine();
                writer.WriteLine($"=== Thiết Bị Tìm Thấy: {scanResults.Count} ===");
                writer.WriteLine();
                writer.WriteLine("Địa chỉ IP".PadRight(16) + "Địa chỉ MAC".PadRight(20) + "Hostname".PadRight(25) + "Port");
                writer.WriteLine(new string('-', 80));
                foreach (var device in scanResults)
                {
                    writer.WriteLine(
                        device.IPAddress.PadRight(16) +
                        device.MacAddress.PadRight(20) +
                        device.HostName.PadRight(25) +
                        string.Join(", ", device.OpenPorts.Select(p => $"{p.PortNumber}({p.Service})"))
                    );
                }
            }
        }

        private void SaveToCsvFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Địa chỉ IP,Địa chỉ MAC,Tên máy chủ,Cổng mở");
                foreach (var device in scanResults)
                {
                    writer.WriteLine(
                        $"\"{device.IPAddress}\"," +
                        $"\"{device.MacAddress}\"," +
                        $"\"{device.HostName}\"," +
                        $"\"{string.Join("; ", device.OpenPorts.Select(p => $"{p.PortNumber}({p.Service})"))}\""
                    );
                }
            }
        }

        private IPAddress GetLocalIPAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }
            }
            return null;
        }

        private IPAddress GetSubnetMask(IPAddress ipAddress)
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.Equals(ipAddress))
                    {
                        return ip.IPv4Mask;
                    }
                }
            }
            return null;
        }

        private IPAddress CalculateNetworkAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();
            byte[] maskBytes = subnetMask.GetAddressBytes();
            byte[] networkBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }
            return new IPAddress(networkBytes);
        }

        private IPAddress CalculateBroadcastAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();
            byte[] maskBytes = subnetMask.GetAddressBytes();
            byte[] broadcastBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                broadcastBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }
            return new IPAddress(broadcastBytes);
        }

        private int CalculateCIDR(IPAddress subnetMask)
        {
            byte[] maskBytes = subnetMask.GetAddressBytes();
            int cidr = 0;
            foreach (byte b in maskBytes)
            {
                int bits = Convert.ToString(b, 2).Count(c => c == '1');
                cidr += bits;
            }
            return cidr;
        }

        private IPAddress GetFirstHostIP(IPAddress networkAddress)
        {
            byte[] bytes = networkAddress.GetAddressBytes();
            bytes[3] += 1;
            return new IPAddress(bytes);
        }

        private IPAddress GetLastHostIP(IPAddress broadcastAddress)
        {
            byte[] bytes = broadcastAddress.GetAddressBytes();
            bytes[3] -= 1;
            return new IPAddress(bytes);
        }

        private async Task<bool> ArpPing(IPAddress ipAddress)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ipAddress, 1000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IcmpPing(IPAddress ipAddress, CancellationToken token)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(ipAddress, 1000);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> GetMacAddressAsync(IPAddress ipAddress)
        {
            try
            {
                byte[] macAddr = new byte[6];
                uint macAddrLen = (uint)macAddr.Length;
                int destIP = BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0);
                int result = await Task.Run(() => SendARP(destIP, 0, macAddr, ref macAddrLen));

                if (result == 0 && macAddrLen == 6)
                {
                    return BitConverter.ToString(macAddr, 0, (int)macAddrLen).Replace("-", ":");
                }

                PhysicalAddress mac = await Task.Run(() => NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(ni => new { Interface = ni, Addresses = ni.GetIPProperties().UnicastAddresses })
                    .Where(x => x.Addresses.Any(ua => ua.Address.Equals(ipAddress)))
                    .Select(x => x.Interface.GetPhysicalAddress())
                    .FirstOrDefault());

                return mac != null && !mac.Equals(PhysicalAddress.None) ? mac.ToString() : "Không xác định";
            }
            catch (Exception ex)
            {
                AppendLog($"Lỗi GetMacAddressAsync cho {ipAddress}: {ex.Message}");
                return "Không xác định";
            }
        }

        private async Task<string> GetHostNameAsync(IPAddress ipAddress)
        {
            try
            {
                IPHostEntry entry = await Dns.GetHostEntryAsync(ipAddress);
                return entry.HostName;
            }
            catch
            {
                return "Không xác định";
            }
        }

        private string GetServiceName(int port)
        {
            switch (port)
            {
                case 21: return "FTP";
                case 22: return "SSH";
                case 23: return "Telnet";
                case 25: return "SMTP";
                case 53: return "DNS";
                case 67: return "DHCP-Server";
                case 68: return "DHCP-Client";
                case 80: return "HTTP";
                case 110: return "POP3";
                case 123: return "NTP";
                case 135: return "RPC";
                case 139: return "NetBIOS";
                case 143: return "IMAP";
                case 161: return "SNMP";
                case 443: return "HTTPS";
                case 445: return "SMB";
                case 500: return "IPsec";
                case 3389: return "RDP";
                case 5060: return "SIP";
                case 8080: return "HTTP-Alt";
                default: return "Không xác định";
            }
        }

        public class NetworkInfo
        {
            public IPAddress LocalIP { get; set; }
            public IPAddress SubnetMask { get; set; }
            public IPAddress NetworkAddress { get; set; }
            public IPAddress BroadcastAddress { get; set; }
            public IPAddress FirstIP { get; set; }
            public IPAddress LastIP { get; set; }
            public int CIDR { get; set; }
            public int TotalIPs { get; set; }
        }

        public class NetworkDevice
        {
            public string IPAddress { get; set; }
            public string MacAddress { get; set; }
            public string HostName { get; set; }
            public List<NetworkPort> OpenPorts { get; set; } = new List<NetworkPort>();
            public DateTime FirstSeen { get; set; }
            public DateTime LastSeen { get; set; }
            public string Status { get; set; }
        }

        public class NetworkPort
        {
            public int PortNumber { get; set; }
            public string Service { get; set; }
        }

        [System.Runtime.InteropServices.DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
    }
}