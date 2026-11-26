using System;
using System.Runtime.InteropServices;

namespace PlasticBand.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    internal readonly struct UInt7 : IEquatable<UInt7>, IComparable<UInt7>
    {
        public static readonly UInt7 MinValue = new UInt7(0);
        public static readonly UInt7 MaxValue = new UInt7(127);

        private readonly byte m_Value;

        public byte Value => m_Value;

        private UInt7(byte value)
        {
            m_Value = value;
        }

        public static UInt7 Clamped(int value)
        {
            return new UInt7((byte)Utilities.Clamp(value, 0, 127));
        }

        public static UInt7 Masked(int value)
        {
            return new UInt7((byte)(value & 0x7F));
        }

        public bool Equals(UInt7 other) => this == other;
        public int CompareTo(UInt7 other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj) => obj is UInt7 other && Equals(other);
        public override int GetHashCode() => m_Value.GetHashCode();
        public override string ToString() => m_Value.ToString();

        public static explicit operator byte(UInt7 value) => value.Value;

        public static bool operator ==(UInt7 left, UInt7 right) => left.Value == right.Value;
        public static bool operator !=(UInt7 left, UInt7 right) => left.Value != right.Value;
        public static bool operator ==(UInt7 left, int right) => left.Value == right;
        public static bool operator !=(UInt7 left, int right) => left.Value != right;
        public static bool operator ==(int left, UInt7 right) => left == right.Value;
        public static bool operator !=(int left, UInt7 right) => left != right.Value;

        // Arithmetic operators return `int` to keep conversions explicit
        public static int operator +(UInt7 value, UInt7 amount) => value.Value + amount.Value;
        public static int operator +(UInt7 value, int amount) => value.Value + amount;
        public static int operator +(int value, UInt7 amount) => value + amount.Value;

        public static int operator -(UInt7 value, UInt7 amount) => value.Value - amount.Value;
        public static int operator -(UInt7 value, int amount) => value.Value - amount;
        public static int operator -(int value, UInt7 amount) => value - amount.Value;

        public static int operator *(UInt7 value, UInt7 amount) => value.Value * amount.Value;
        public static int operator *(UInt7 value, int amount) => value.Value * amount;
        public static int operator *(int value, UInt7 amount) => value * amount.Value;

        public static int operator /(UInt7 value, UInt7 amount) => value.Value / amount.Value;
        public static int operator /(UInt7 value, int amount) => value.Value / amount;
        public static int operator /(int value, UInt7 amount) => value / amount.Value;

        public static int operator %(UInt7 value, UInt7 amount) => value.Value % amount.Value;
        public static int operator %(UInt7 value, int amount) => value.Value % amount;
        public static int operator %(int value, UInt7 amount) => value % amount.Value;

        // Same with bitwise operators
        public static int operator &(UInt7 value, UInt7 amount) => value.Value & amount.Value;
        public static int operator &(UInt7 value, int amount) => value.Value & amount;
        public static int operator &(int value, UInt7 amount) => value & amount.Value;

        public static int operator |(UInt7 value, UInt7 amount) => value.Value | amount.Value;
        public static int operator |(UInt7 value, int amount) => value.Value | amount;
        public static int operator |(int value, UInt7 amount) => value | amount.Value;

        public static int operator ^(UInt7 value, UInt7 amount) => value.Value ^ amount.Value;
        public static int operator ^(UInt7 value, int amount) => value.Value ^ amount;
        public static int operator ^(int value, UInt7 amount) => value ^ amount.Value;

        public static int operator <<(UInt7 value, int amount) => value.Value << amount;
        public static int operator >>(UInt7 value, int amount) => value.Value >> amount;

        public static int operator ~(UInt7 value) => ~value.Value & 0x7F;
    }
}
