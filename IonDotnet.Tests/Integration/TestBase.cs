using System;
using System.IO;
using System.Text;
using IonDotnet.Systems;
using IonDotnet.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IonDotnet.Tests.Integration
{
    public abstract class TestBase
    {
        public enum InputStyle
        {
            MemoryStream,
            FileStream,
            Text,
            NoSeekStream
        }

        private Stream _stream;

        [TestCleanup]
        public void Cleanup()
        {
            _stream?.Dispose();
        }

        protected IIonReader ReaderFromFile(FileInfo file, InputStyle style)
        {
            _stream?.Dispose();
            switch (style)
            {
                case InputStyle.MemoryStream:
                    var bytes = File.ReadAllBytes(file.FullName);
                    _stream = new MemoryStream(bytes);
                    return IonReaderBuilder.Build(_stream);
                case InputStyle.FileStream:
                    _stream = file.OpenRead();
                    return IonReaderBuilder.Build(_stream);
                case InputStyle.Text:
                    var str = File.ReadAllText(file.FullName, Encoding.UTF8);
                    return IonReaderBuilder.Build(str);
                case InputStyle.NoSeekStream:
                    var b = File.ReadAllBytes(file.FullName);
                    _stream = new NoSeekMemStream(b);
                    return IonReaderBuilder.Build(_stream);
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }
    }
}
