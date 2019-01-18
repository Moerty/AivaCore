using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Enums;

namespace Aiva.Core {
    public static class Helpers {
        public static bool IsUserPermitted(this UserType userType) {
            return userType != UserType.Viewer && userType != UserType.Staff;
        }
    }
}
