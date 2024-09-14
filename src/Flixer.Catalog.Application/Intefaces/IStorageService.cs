﻿namespace Flixer.Catalog.Application.Intefaces;

public interface IStorageService
{
    Task Delete(string filePath);
    Task<string> Upload(string fileName, Stream fileStream);
}