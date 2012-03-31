watch('.*.\.cs$') do
  puts `build.bat`
  puts `nspec.bat`
end
