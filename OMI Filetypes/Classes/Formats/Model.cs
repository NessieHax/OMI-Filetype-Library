﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * all known Model/Material information is the direct product of May/MattNL's work! check em out! 
 * https://github.com/MattN-L
*/
namespace OMI.Formats.Model
{
    public class ModelContainer
    {

        public Dictionary<string, ModelPiece> models = new Dictionary<string, ModelPiece>();


        public ModelContainer()
        {

        }


    }

    public class ModelPiece
    {
        public int TextureWidth;
        public int TextureHeight;
        public Dictionary<string, ModelPart> Parts = new Dictionary<string, ModelPart>();
    }

    public class ModelPart
    {
        public float UnknownFloat;
        public float TranslationX;
        public float TranslationY;
        public float TranslationZ;
        public float TextureOffsetX;
        public float TextureOffsetY;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public Dictionary<string, ModelBox> Boxes = new Dictionary<string, ModelBox>();
    }

    public class ModelBox
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public int Length;
        public int Width;
        public int Height;
        public float UvX;
        public float UvY;
        public float Scale;
        public bool Mirror;
    }
}