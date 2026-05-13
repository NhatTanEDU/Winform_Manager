using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using RMA.Server.Entities;

namespace RMA.Server.Data
{
    public class RmaCsvRecord
    {
        [Name("Khách hàng")]
        public string? CustomerName { get; set; }

        [Name("Thông Tin KH")]
        public string? CustomerInfo { get; set; }

        [Name("Tên TB")]
        public string? DeviceName { get; set; }

        [Name("Serial/Cấu Hình")]
        public string? SerialNumber { get; set; }

        [Name("Phụ Kiện")]
        public string? Accessories { get; set; }

        [Name("Ngày nhận")]
        public string? ReceivedDate { get; set; }

        [Name("Chế Độ")]
        public string? ServiceMode { get; set; }

        [Name("Ngày bảo hành")]
        public string? WarrantyDate { get; set; }

        [Name("Lỗi KH báo")]
        public string? CustomerProblem { get; set; }

        [Name("Thực tế kiểm tra")]
        public string? ActualCheck { get; set; }

        [Name("Hướng khắc phục")]
        public string? Solution { get; set; }

        [Name("Tên NCC")]
        public string? VendorName { get; set; }

        [Name("Người liên hệ")]
        public string? VendorContact { get; set; }

        [Name("Ngày gửi NCC")]
        public string? SentToVendorDate { get; set; }

        [Name("ĐV Vận chuyển")]
        public string? ShippingUnit { get; set; }

        [Name("Người Phụ Trách")]
        public string? Assignee { get; set; }

        [Name("Tình trạng TB sau sửa chữa")]
        public string? FinalStatus { get; set; }

        [Name("Vị Trí")]
        public string? Location { get; set; }

        [Name("Tình trạng thiết bị")]
        public string? DeviceCondition { get; set; }

        [Name("Ghi chú ")]
        public string? Notes { get; set; }
    }

    public static class CsvImporter
    {
        public static void ImportData(ApplicationDbContext context, string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine($"CSV file not found at: {csvFilePath}");
                return;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                BadDataFound = null,
                IgnoreBlankLines = true,
            };

            List<RmaCsvRecord> records;
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                records = csv.GetRecords<RmaCsvRecord>().ToList();
            }

            Console.WriteLine($"Found {records.Count} records in CSV. Starting import...");

            // Get/Create basic Categories
            var categories = context.Categories.ToList();
            if (!categories.Any())
            {
                categories.Add(new Category { Name = "Laptop" });
                categories.Add(new Category { Name = "Desktop" });
                categories.Add(new Category { Name = "Monitor" });
                categories.Add(new Category { Name = "Accessories" });
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Get/Create StatusMasters
            var statuses = context.StatusMasters.ToList();
            if (!statuses.Any())
            {
                statuses.Add(new StatusMaster { StatusName = "New", ColorCode = "#0000FF" });
                statuses.Add(new StatusMaster { StatusName = "In Progress", ColorCode = "#FFA500" });
                statuses.Add(new StatusMaster { StatusName = "Completed", ColorCode = "#008000" });
                context.StatusMasters.AddRange(statuses);
                context.SaveChanges();
            }

            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.CustomerName) && string.IsNullOrWhiteSpace(record.SerialNumber))
                {
                    continue; // Skip empty rows
                }

                // 1. Process Customer
                var customerName = string.IsNullOrWhiteSpace(record.CustomerName) ? "Unknown Customer" : record.CustomerName.Trim();
                var customer = context.Customers.FirstOrDefault(c => c.Name == customerName);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = customerName,
                        Phone = record.CustomerInfo?.Length > 20 ? record.CustomerInfo.Substring(0, 20) : record.CustomerInfo,
                        Address = record.CustomerInfo?.Length > 500 ? record.CustomerInfo.Substring(0, 500) : record.CustomerInfo,
                    };
                    context.Customers.Add(customer);
                    context.SaveChanges(); // Save to get ID
                }

                // 2. Process Model & Category
                var deviceName = string.IsNullOrWhiteSpace(record.DeviceName) ? "Unknown Device" : record.DeviceName.Trim();
                var model = context.Models.FirstOrDefault(m => m.ModelName == deviceName);
                if (model == null)
                {
                    // Infer category
                    string catName = "Accessories";
                    string lowerName = deviceName.ToLower();
                    if (lowerName.Contains("laptop") || lowerName.Contains("thinkpad") || lowerName.Contains("macbook"))
                        catName = "Laptop";
                    else if (lowerName.Contains("màn hình") || lowerName.Contains("lcd") || lowerName.Contains("monitor") || lowerName.Contains("dell u") || lowerName.Contains("dell p"))
                        catName = "Monitor";
                    else if (lowerName.Contains("pc") || lowerName.Contains("desktop"))
                        catName = "Desktop";

                    var category = categories.FirstOrDefault(c => c.Name == catName) ?? categories.First();

                    model = new Model
                    {
                        ModelName = deviceName,
                        Brand = "Unknown",
                        CategoryId = category.Id
                    };
                    context.Models.Add(model);
                    context.SaveChanges();
                }

                // 3. Process Vendor
                Vendor? vendor = null;
                if (!string.IsNullOrWhiteSpace(record.VendorName))
                {
                    var vendorName = record.VendorName.Trim();
                    vendor = context.Vendors.FirstOrDefault(v => v.Name == vendorName);
                    if (vendor == null)
                    {
                        vendor = new Vendor
                        {
                            Name = vendorName,
                            ContactInfo = record.VendorContact?.Length > 500 ? record.VendorContact.Substring(0, 500) : record.VendorContact
                        };
                        context.Vendors.Add(vendor);
                        context.SaveChanges();
                    }
                }

                // 4. Process Device
                var serial = string.IsNullOrWhiteSpace(record.SerialNumber) ? "Unknown-SN-" + Guid.NewGuid().ToString().Substring(0, 8) : record.SerialNumber.Trim();
                // Ensure Serial isn't too long
                if (serial.Length > 100) serial = serial.Substring(0, 100);

                var device = context.Devices.FirstOrDefault(d => d.SerialNumber == serial);
                if (device == null)
                {
                    DateTime? warrantyDate = ParseDate(record.WarrantyDate);

                    device = new Device
                    {
                        SerialNumber = serial,
                        CustomerId = customer.Id,
                        ModelId = model.Id,
                        WarrantyExpiry = warrantyDate
                    };
                    context.Devices.Add(device);
                    context.SaveChanges();
                }

                // 5. Process Ticket Status
                var statusStr = record.FinalStatus?.ToLower().Trim() ?? "";
                string targetStatusName = "New";
                if (statusStr.Contains("done") || statusStr.Contains("xong") || statusStr == "trả khách")
                    targetStatusName = "Completed";
                else if (statusStr.Contains("gọi bh") || statusStr.Contains("đang") || statusStr.Contains("chờ"))
                    targetStatusName = "In Progress";

                var statusMaster = statuses.FirstOrDefault(s => s.StatusName == targetStatusName) ?? statuses.First();

                // 6. Assemble StaffNote
                var staffNotes = new List<string>();
                if (!string.IsNullOrWhiteSpace(record.Accessories)) staffNotes.Add($"Phụ kiện: {record.Accessories}");
                if (!string.IsNullOrWhiteSpace(record.ActualCheck)) staffNotes.Add($"Kiểm tra: {record.ActualCheck}");
                if (!string.IsNullOrWhiteSpace(record.Solution)) staffNotes.Add($"Khắc phục: {record.Solution}");
                if (!string.IsNullOrWhiteSpace(record.ShippingUnit)) staffNotes.Add($"Vận chuyển: {record.ShippingUnit}");
                if (!string.IsNullOrWhiteSpace(record.Assignee)) staffNotes.Add($"Phụ trách: {record.Assignee}");
                if (!string.IsNullOrWhiteSpace(record.Location)) staffNotes.Add($"Vị trí: {record.Location}");
                if (!string.IsNullOrWhiteSpace(record.DeviceCondition)) staffNotes.Add($"Tình trạng TB: {record.DeviceCondition}");
                if (!string.IsNullOrWhiteSpace(record.Notes)) staffNotes.Add($"Ghi chú: {record.Notes}");
                
                string combinedNotes = string.Join(" | ", staffNotes);
                if (combinedNotes.Length > 2000) combinedNotes = combinedNotes.Substring(0, 2000);

                // 7. Check for existing ticket to avoid duplicates
                var receivedDate = ParseDate(record.ReceivedDate) ?? DateTime.UtcNow;
                var problemDesc = string.IsNullOrWhiteSpace(record.CustomerProblem) ? "Không có mô tả lỗi" : record.CustomerProblem;
                if (problemDesc.Length > 2000) problemDesc = problemDesc.Substring(0, 2000);

                var existingTicket = context.RmaTickets.FirstOrDefault(t => 
                    t.DeviceId == device.Id && 
                    t.CustomerId == customer.Id && 
                    t.ReceivedDate.Date == receivedDate.Date);

                if (existingTicket == null)
                {
                    var rmaTicket = new RmaTicket
                    {
                        DeviceId = device.Id,
                        CustomerId = customer.Id,
                        StatusId = statusMaster.Id,
                        VendorId = vendor?.Id,
                        ProblemDescription = problemDesc,
                        ServiceMode = record.ServiceMode?.Length > 100 ? record.ServiceMode.Substring(0, 100) : record.ServiceMode,
                        ReceivedDate = receivedDate,
                        SentDate = ParseDate(record.SentToVendorDate),
                        StaffNote = combinedNotes,
                        IsUrgent = record.FinalStatus?.ToLower().Contains("urgency") ?? false
                    };

                    context.RmaTickets.Add(rmaTicket);
                }
                else
                {
                    // Optionally update existing ticket if status or notes changed
                    existingTicket.StatusId = statusMaster.Id;
                    existingTicket.StaffNote = combinedNotes;
                    existingTicket.VendorId = vendor?.Id;
                    existingTicket.SentDate = ParseDate(record.SentToVendorDate);
                    context.RmaTickets.Update(existingTicket);
                }
            }

            context.SaveChanges();
            Console.WriteLine("Data import/update completed successfully!");
        }

        private static DateTime? ParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;
            
            dateStr = dateStr.Trim();
            
            // Basic parsing attempts for common vn formats
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd" };
            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            if (DateTime.TryParse(dateStr, out parsedDate))
            {
                return parsedDate;
            }

            return null; // Return null if it's invalid like 'Expired' or contains other text
        }
    }
}
