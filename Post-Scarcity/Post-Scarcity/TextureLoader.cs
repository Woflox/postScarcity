using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Post_Scarcity
{
    public static class TextureLoader
    {
        public static Dictionary<string, Texture2D> textures = new Dictionary<string,Texture2D>();

        public static Texture2D LoadTexture(string imageID)
        {
            if (textures.ContainsKey(imageID))
            {
                return textures[imageID];
            }
            else
            {
                textures[imageID] = Game1.instance.Content.Load<Texture2D>(imageID);
                return textures[imageID];
            }
        }
    }
}