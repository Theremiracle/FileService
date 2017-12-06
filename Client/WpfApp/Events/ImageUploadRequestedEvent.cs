using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WpfApp.Events
{
    public class ImageUploadRequestedEvent : PubSubEvent<Action<Byte[]>>
    {
    }
}
