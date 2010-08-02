version = File.read(File.expand_path("../VERSION",__FILE__)).strip

Gem::Specification.new do |s|
  s.platform    = Gem::Platform::RUBY
  s.name        = 'warmup'
  s.version     = version
  s.files = Dir['lib/**/*'] + Dir['bin/**/*']
  s.bindir = 'bin'
  s.executables << 'warmup'
  
  s.summary     = 'WarmuP - Template your Solution!'
  s.description = 'WarmuP - Your Templates, your choices!'
  
  s.authors            = ['Dru Sellers','Rob Reynolds']
  s.email             = 'chucknorrisframework@googlegroups.com'
  s.homepage          = 'http://groups.google.com/group/chucknorrisframework'
  s.rubyforge_project = 'warmup'
end