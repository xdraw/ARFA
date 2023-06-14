using UnityEngine;

namespace ArFight.Scripts
{
    public interface IHasVuforiaTarget
    {
        Texture2D TargetTexture
        {
            get;
        }

        string TargetName
        {
            get;
        }
    }
}