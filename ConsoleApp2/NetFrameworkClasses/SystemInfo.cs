using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace NetFrameworkClasses
{
    public class SystemInfo
    {
        // Метод для получения системной идентификации и имени компьютера
        public static void GetSystemIdentification()
        {
            Console.WriteLine("System Identification:");
            Console.WriteLine($"Computer Name: {Environment.MachineName}");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"OS Name: {obj["Caption"]}");
                Console.WriteLine($"OS Version: {obj["Version"]}");
                Console.WriteLine($"Serial Number: {obj["SerialNumber"]}");
            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"UUID: {obj["UUID"]}");
            }
        }

        // Метод для получения информации о сетевых адаптерах и их MAC адресах
        public static void GetNetworkAdapters()
        {
            Console.WriteLine("\nNetwork Adapters:");

            int count = 0;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"Description: {obj["Description"]}");
                Console.WriteLine($"MAC Address: {obj["MACAddress"]}\n");

                count++;
            }

            Console.WriteLine($"Count Network adapters: {count} \n");
        }

        // Метод для получения модели процессора
        public static void GetProcessorInfo()
        {
            Console.WriteLine("Processor Information:");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"Name: {obj["Name"]}");
                Console.WriteLine($"Manufacturer: {obj["Manufacturer"]}");
                Console.WriteLine($"Description: {obj["Description"]}\n");
            }
        }

        // Метод для получения объема оперативной памяти
        public static void GetMemoryInfo()
        {
            Console.WriteLine("Memory Information:");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"Total Physical Memory: {Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / 1048576, 2)} MB\n");
            }
        }

        // Метод для получения модели материнской платы
        public static void GetMotherboardInfo()
        {
            Console.WriteLine("Motherboard Information:");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"Model: {obj["Product"]}");
                Console.WriteLine($"Manufacturer: {obj["Manufacturer"]}");
                Console.WriteLine($"Serial Number: {obj["SerialNumber"]}\n");
            }
        }

        // Метод для получения информации о логических томах
        public static void GetLogicalDrivesInfo()
        {
            Console.WriteLine("Logical Drives Information:");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject obj in searcher.Get())
            {
                Console.WriteLine($"Name: {obj["Name"]}");
                Console.WriteLine($"File System: {obj["FileSystem"]}");
                Console.WriteLine($"Total Size: {Math.Round(Convert.ToDouble(obj["Size"]) / 1073741824, 2)} GB");
                Console.WriteLine($"Free Space: {Math.Round(Convert.ToDouble(obj["FreeSpace"]) / 1073741824, 2)} GB\n");
            }
        }
    }
}
