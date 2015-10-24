using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization
{
    /// <summary>
    /// Represents a permission <see cref="Name"/> with <see cref="IsGranted"/> information.
    /// </summary>
    public class PermissionGrantInfo
    {
        /// <summary>
        /// Name of the permission.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is this permission granted?
        /// </summary>
        public bool IsGranted { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="PermissionGrantInfo"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isGranted"></param>
        public PermissionGrantInfo(string name, bool isGranted)
        {
            Name = name;
            IsGranted = isGranted;
        }
    }
}
