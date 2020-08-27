﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Parchegram.Service.ClassesSupport
{
    public class Image
    {
        /// <summary>
        /// Sobrecarga privada para obtener un archivo en byte[] directamente de un IFormFile
        /// </summary>
        /// <param name="formFile">Archivo que se llego a un controlador</param>
        /// <returns>Archivo en byte[]/returns>
        public byte[] GetFile(IFormFile formFile)
        {
            byte[] fileBytes = null;
            if (formFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    formFile.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }
            }

            return fileBytes;
        }

        /// <summary>
        /// Obtener archivo en byte[] desde un path del servidor
        /// </summary>
        /// <param name="fullPath">Path que contiene la ubicación del archivo</param>
        /// <returns>Archivo en byte[]</returns>
        public async Task<byte[]> GetFile(string fullPath)
        {
            byte[] result;
            using (FileStream file = File.Open(fullPath, FileMode.Open))
            {
                result = new byte[file.Length];
                await file.ReadAsync(result, 0, (int)file.Length);
            }

            return result;
        }
    }
}
