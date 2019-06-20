﻿using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace OpenCvSharp.Tests.Text
{
    public class TextDetectorTest : TestBase
    {
        private const string modelArch = "_data/text/textbox.prototxt";
        private const string modelWeights = "_data/model/TextBoxes_icdar13.caffemodel";
        private static readonly string modelWeightsUrl = string.Format("https://drive.google.com/uc?id={0}&export=download", "10rqbOxZphuwk0TaWCaixIhheIBnxoaxv"); 
        
        public TextDetectorTest()
        { 
            if (!File.Exists(modelWeights))
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = delegate{ return true; }
                };
                using (var client = new HttpClient(handler))
                {
                    var data = client.GetByteArrayAsync(modelWeightsUrl).GetAwaiter().GetResult();
                    File.WriteAllBytes(modelWeights, data);
                }
            }
        }

        [Fact]
        public void Create()
        {
            Assert.True(File.Exists(modelArch), $"modelArch '{modelArch}' not found");
            Assert.True(File.Exists(modelWeights), $"modelWeights '{modelWeights}' not found");
            Assert.True(new FileInfo(modelWeights).Length > 10_000_000);

            using (var detector = TextDetectorCNN.Create(modelArch, modelWeights))
            {
                GC.KeepAlive(detector);
            }
        }
    }
}
