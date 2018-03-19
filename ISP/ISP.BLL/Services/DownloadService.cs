using System.Text;
using ISP.BLL.Interfaces;
using ISP.DAL.Interfaces;

namespace ISP.BLL.Services
{
    public class DownloadService : IDownloadService
    {
        private IUnitOfWork Database { get; }

        public DownloadService(IUnitOfWork database)
        {
            Database = database;
        }

        public byte[] GetServices()
        {
            var services = Database.ServicesRepository.GetAll();
            var outputBuilder = new StringBuilder();

            foreach (var service in services)
            {
                outputBuilder.AppendLine($"Service: <{service.Title}>. Plans:");
                var servicePlans = Database.PlansRepository.Find(t => t.ServiceId == service.Id);
                foreach (var servicePlan in servicePlans)
                {
                    if (servicePlan.IsDeleted)
                    {
                        continue;
                    }
                    outputBuilder.AppendLine($"-Plan: {servicePlan.Title}:");
                    foreach (var servicePlanFeature in servicePlan.Features)
                    {
                        if (servicePlanFeature.IsDeleted)
                        {
                            continue;
                        }
                        outputBuilder.AppendLine($"--Feature: {servicePlanFeature.Title}: {servicePlanFeature.Description}");
                    }
                }
                outputBuilder.AppendLine();
                outputBuilder.AppendLine();
            }

            var result = outputBuilder.ToString();
            return GetBytes(result);
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
