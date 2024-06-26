﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace OMI.Formats.Pck
{
    public class PckAsset : IEquatable<PckAsset>
    {
        public string Filename
        {
            get => filename;
            set
            {
                string newFilename = value.Replace('\\', '/');
                OnFilenameChanging?.Invoke(this, newFilename);
                filename = newFilename;
            }
        }
        public PckAssetType Type
        {
            get => type;
            set
            {
                var newValue = value;
                OnAssetTypeChanging?.Invoke(this, newValue);
                type = newValue;
            }
        }

        public byte[] Data => _data;
        public int Size => _data?.Length ?? 0;

        public int PropertyCount => Properties.Count;

        public PckAsset(string filename, PckAssetType type)
        {
            Type = type;
            Filename = filename;
        }

        public void AddProperty(KeyValuePair<string, string> property) => Properties.Add(property);

        public void AddProperty(string name, string value) => Properties.Add(name, value);

        public void AddProperty<T>(string name, T value) => Properties.Add(name, value);

        public void RemoveProperty(string name) => Properties.Remove(name);

        public bool RemoveProperty(KeyValuePair<string, string> property) => Properties.Remove(property);

        public void RemoveProperties(string name) => Properties.RemoveAll(p => p.Key == name);

        public void ClearProperties() => Properties.Clear();

        public bool HasProperty(string property) => Properties.Contains(property);

        public int GetPropertyIndex(KeyValuePair<string, string> property) => Properties.IndexOf(property);

        public string GetProperty(string name) => Properties.GetPropertyValue(name);

        public T GetProperty<T>(string name, Func<string, T> func) => Properties.GetPropertyValue(name, func);

        public bool TryGetProperty(string name, out string value) => Properties.TryGetProperty(name, out value);

        public KeyValuePair<string, string>[] GetMultipleProperties(string property) => Properties.GetProperties(property);

        public IReadOnlyList<KeyValuePair<string, string>> GetProperties() => Properties.AsReadOnly();

        public void SetProperty(int index, KeyValuePair<string, string> property) => Properties[index] = property;

        public void SetProperty(string name, string value) => Properties.SetProperty(name, value);

        public override bool Equals(object obj)
        {
            return obj is PckAsset other && Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = 953938382;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Filename);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Data);
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<PckFileProperties>.Default.GetHashCode(Properties);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(filename);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(_data);
            return hashCode;
        }

        public void SetData(byte[] data)
        {
            _data = data;
        }

        internal delegate void OnFilenameChangingDelegate(PckAsset _this, string newFilename);
        internal delegate void OnFiletypeChangingDelegate(PckAsset _this, PckAssetType newFiletype);
        internal delegate void OnMoveDelegate(PckAsset _this);
        internal PckFileProperties Properties = new PckFileProperties();

        private string filename;
        private PckAssetType type;
        private OnFilenameChangingDelegate OnFilenameChanging;
        private OnFiletypeChangingDelegate OnAssetTypeChanging;
        private OnMoveDelegate OnMove;
        private byte[] _data = new byte[0];

        internal PckAsset(string filename, PckAssetType filetype,
            OnFilenameChangingDelegate onFilenameChanging, OnFiletypeChangingDelegate onFiletypeChanging,
            OnMoveDelegate onMove)
            : this(filename, filetype)
        {
            SetEvents(onFilenameChanging, onFiletypeChanging, onMove);
        }

        internal PckAsset(string filename, PckAssetType filetype, int dataSize) : this(filename, filetype)
        {
            _data = new byte[dataSize];
        }

        internal bool HasEventsSet()
        {
            return OnFilenameChanging != null && OnAssetTypeChanging != null && OnMove != null;
        }

        internal void SetEvents(OnFilenameChangingDelegate onFilenameChanging, OnFiletypeChangingDelegate onFiletypeChanging, OnMoveDelegate onMove)
        {
            OnFilenameChanging = onFilenameChanging;
            OnAssetTypeChanging = onFiletypeChanging;
            OnMove = onMove;
        }

        public bool Equals(PckAsset other)
        {
            var hasher = MD5.Create();
            var thisHash = BitConverter.ToString(hasher.ComputeHash(Data));
            var otherHash = BitConverter.ToString(hasher.ComputeHash(other.Data));
            return Filename.Equals(other.Filename) &&
                Type.Equals(other.Type) &&
                Size.Equals(other.Size) &&
                thisHash.Equals(otherHash);
        }

        internal void Move()
        {
            OnMove?.Invoke(this);
        }
    }
}
