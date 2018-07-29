# ImHash
 An .NET image hasher library for comparing the similarity between images using perceptual hash. Inspired by: https://github.com/jenssegers/imagehash.

# Documenation
```csharp
int tolerance = 8;
avgHasher = new ImAvgHash(tolerance);
diffHasher = new ImDiffHash(tolerance);

bool avgHasherResult = new ImAvgHash(tolerance);
bool diffHasherResult = new ImDiffHash(tolerance);
```

# Installation
     Install-Package ImHash
https://www.nuget.org/packages/ImHash/

# Explanation
https://www.linkedin.com/pulse/imhash-imhasher-ghasan-al-sakkaf/
