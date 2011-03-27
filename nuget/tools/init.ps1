param($installPath, $toolsPath, $package, $project)

Write-Host 'This is not a package with dlls to reference in it.'
Write-Host 'Please install using chocolatey'
Write-Host 'chocolatey install warmup'
write-host 'Removing this package...'
#uninstall-package $package.Name -ProjectName $project.Name
uninstall-package warmup