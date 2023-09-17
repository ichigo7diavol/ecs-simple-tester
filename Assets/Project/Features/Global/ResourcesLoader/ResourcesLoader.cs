using System;
using Cysharp.Threading.Tasks;

namespace Project.Features.Global.ResourcesLoader
{
    public class ResourcesLoader
    {
        public async UniTask LoadResource()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5));
        }
    }
}