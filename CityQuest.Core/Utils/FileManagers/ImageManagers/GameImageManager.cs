using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CityQuest.Utils.FileManagers.ImageManagers
{
    public class GameImageManager: ITransientDependency
    {
        public string DirectoryPath = String.Format("{0}{1}", System.Web.Hosting.HostingEnvironment.MapPath("~/"), CityQuestConsts.GameImagesStorePath);

        public GameImageManager()
        {

        }

        public void SaveImage(string imageName, string imageExtension, byte[] imageData)
        {
            try
            {
                string destination = Path.Combine(DirectoryPath, String.Format("{0}{1}", imageName, imageExtension));
                if (File.Exists(destination))
                {
                    // If file exists - removing it
                    File.Delete(destination);
                }
                using (var imageFile = new FileStream(destination, System.IO.FileMode.Create))
                {
                    imageFile.Write(imageData, 0, imageData.Length);
                    imageFile.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Can not save game image file {0}.{1}! ({2})", imageName, imageExtension, ex));
            }
        }

        public void RemoveImage(string imageNameWithExtension)
        {
            try
            {
                string destination = Path.Combine(DirectoryPath, imageNameWithExtension);
                if (File.Exists(destination))
                {
                    // If file exists - removing it
                    File.Delete(destination);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Can not remove game image file {0}! ({1})", imageNameWithExtension, ex));
            }
        }
    }
}
