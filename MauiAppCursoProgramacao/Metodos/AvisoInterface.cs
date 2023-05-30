using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppCursoProgramacao.Metodos
{
    public class AvisoInterface
    {
        public async Task ShakeInterface(VerticalStackLayout stackLayout)
        {
            const int shakeDuration = 50;
            const int shakeRotation = 5;

            await stackLayout.RotateTo(-shakeRotation, shakeDuration);
            await stackLayout.RotateTo(shakeRotation, shakeDuration * 2);
            await stackLayout.RotateTo(-shakeRotation, shakeDuration * 2);
            await stackLayout.RotateTo(shakeRotation, shakeDuration * 2);
            await stackLayout.RotateTo(0, shakeDuration);
        }


    }
}
