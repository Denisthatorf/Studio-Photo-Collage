using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumia.Imaging;
using Lumia.Imaging.Workers;

namespace Studio_Photo_Collage.Infrastructure.Effects
{
    public class RedColorEffect : EffectBase
    {
        public override RenderOptions SupportedRenderOptions => throw new NotImplementedException();

        public override IImageProvider2 Clone() => throw new NotImplementedException();
        public override IImageWorker CreateImageWorker(IImageWorkerRequest imageWorkerRequest) => throw new NotImplementedException();
    }
}
