using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace NetFrameworkClasses
{
    public class Info
    {
        public List<List<string>> Parameters { get; set; } = new List<List<string>>();

        public Info()
        {
            Parameters.Add(GetSystemIdentification());
            Parameters.Add(GetNetworkAdapters());
            Parameters.Add(GetProcessorInfo());
            Parameters.Add(GetMemoryInfo());
            Parameters.Add(GetMotherboardInfo());
            Parameters.Add(GetLogicalDrivesInfo());
        }

        public override string ToString()
        {
            string str = "";
            foreach (List<string> parameter in this.Parameters)
            {

                foreach (var line in parameter)
                {
                    str += Environment.NewLine + line;
                }
                str += Environment.NewLine;

            }
            return str;
        }
        // Метод для получения системной идентификации и имени компьютера
        public List<string> GetSystemIdentification()
        {
            List<string> parameters = new List<string>();

            parameters.Add(Environment.MachineName.ToString());

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add("OS Name: " + obj["Caption"].ToString());
                parameters.Add("OS Version: " + obj["Version"].ToString());
                parameters.Add("Serial Number: " + obj["SerialNumber"].ToString());


            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add("UUID: " + obj["UUID"].ToString());
            }
            return parameters;
        }

        // Метод для получения информации о сетевых адаптерах и их MAC адресах
        public List<string> GetNetworkAdapters()
        {
            List<string> parameters = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add("Description: " + obj["Description"].ToString() + ", MAC Address: " + obj["MACAddress"].ToString());
            }
            return parameters;
        }

        // Метод для получения модели процессора
        public List<string> GetProcessorInfo()
        {
            List<string> parameters = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add("Name: " + obj["Name"].ToString());

                parameters.Add("Manufacturer: " + obj["Manufacturer"].ToString());

                parameters.Add("Description: " + obj["Description"].ToString());

            }
            return parameters;
        }

        // Метод для получения объема оперативной памяти
        public List<string> GetMemoryInfo()
        {

            List<string> parameters = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add($"Total Physical Memory: {Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / 1048576, 2)} MB");
            }
            return parameters;
        }

        // Метод для получения модели материнской платы
        public List<string> GetMotherboardInfo()
        {
            List<string> parameters = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add("Model: " + obj["Product"].ToString());

                parameters.Add("Manufacturer: " + obj["Manufacturer"].ToString());

                parameters.Add("Serial Number: " + obj["SerialNumber"].ToString());

            }
            return parameters;
        }

        // Метод для получения информации о логических томах
        public List<string> GetLogicalDrivesInfo()
        {
            List<string> parameters = new List<string>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject obj in searcher.Get())
            {
                parameters.Add($"Name: " + obj["Name"].ToString() + ", " +
                                "File System: " + obj["FileSystem"].ToString() + ", " +
                               $"Total Size: {Math.Round(Convert.ToDouble(obj["Size"]) / 1073741824, 2)} GB" + ", " +
                               $"Free Space: {Math.Round(Convert.ToDouble(obj["FreeSpace"]) / 1073741824, 2)} GB");
            }
            return parameters;
        }
    }
}
