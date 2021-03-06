﻿using Tyml.Serialization;

namespace Hediet.KeyboardMapper.Config
{
    public enum QwertzModifier
    {
        Shift, Alt, Strg
    }

    [TymlObjectType]
    public class KeyDefinition
    {
        [CanBeImplicit]
        public string Name { get; set; }
        public string Text { get; set; }

        public int? QwertzScanCode { get; set; }
        public int? QwertzVirtualKeyCode { get; set; }

        public QwertzModifier[] Modifiers { get; set; }
    }
}