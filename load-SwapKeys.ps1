$assemblyPath = "$pwd/Win32-Hook/Win32Hook.dll"
[System.Reflection.Assembly]::LoadFile($assemblyPath);

$srcSwapKeys = get-content -raw swapKeys.cs

add-type                             `
  -typeDefinition       $srcSwapKeys `
  -referencedAssemblies              `
     System.Windows.Forms,           `
     $assemblyPath

[TQ84.SwapKeys]::go()
