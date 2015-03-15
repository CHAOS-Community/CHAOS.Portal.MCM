require 'bundler/setup'
require 'fileutils'

require 'albacore'
require 'albacore/tasks/versionizer'

Configuration = ENV['CONFIGURATION'] || 'Release'

Albacore::Tasks::Versionizer.new :versioning

task :clean do
  FileUtils.rmtree 'tests'
  FileUtils.rmtree 'build'
end

desc 'create assembly infos'
asmver_files :assembly_info do |a|
  a.files = FileList['**/*proj'] # optional, will find all projects recursively by default

  a.attributes assembly_description: 'TODO',
               assembly_configuration: Configuration,
               assembly_company: 'CHAOS APS',
               assembly_copyright: "(c) 2015 by CHAOS APS",
               assembly_version: ENV['LONG_VERSION'],
               assembly_file_version: ENV['LONG_VERSION'],
               assembly_informational_version: ENV['BUILD_VERSION']
end

task :prepare_compile => [:clean] do
  FileUtils.cp 'src/app/Chaos.Mcm.Indexer/App.config.sample', 'src/app/Chaos.Mcm.Indexer/App.config'
end

desc 'Perform fast build (warn: doesn\'t d/l deps)'
build :quick_compile => [:prepare_compile] do |b|
  b.prop 'Configuration', Configuration
  b.logging = 'quiet'
  b.sln     = 'MCM.sln'
end

task :package_tests => [:quick_compile] do
  FileUtils.mkdir 'tests'

  FileUtils.cp 'src\app\CHAOS.MCM\bin\Release\CHAOS.MCM.dll', 'tests'

  FileUtils.cp 'lib\CHAOS.dll', 'tests'
  FileUtils.cp 'lib\Chaos.Portal.dll', 'tests'
  FileUtils.cp 'packages/Newtonsoft.Json.6.0.5/lib/net45/Newtonsoft.Json.dll', 'tests'
  FileUtils.cp 'packages/Moq.4.2.1502.0911/lib/NET40/Moq.dll', 'tests'
  FileUtils.cp 'packages/NUnit.2.6.4/lib/nunit.framework.dll', 'tests'
  FileUtils.cp 'packages/AWSSDK.2.3.24.3/lib/NET45/AWSSDK.dll', 'tests'
  FileUtils.cp 'packages/CouchbaseNetClient.1.3.9/lib/NET40/Couchbase.dll', 'tests'

  system 'tools/ILMerge/ILMerge.exe',
    ['/out:tests\Chaos.Portal.Mcm.Test.dll',
     '/target:library',
     '/ndebug',
     '/lib:lib',
     '/targetplatform:v4,c:\windows\Microsoft.Net\Framework64\v4.0.30319',
     '/lib:c:\windows\Microsoft.Net\Framework64\v4.0.30319',
     'src\test\CHAOS.MCM.Test\bin\Release\CHAOS.MCM.Test.dll'], clr_command: true
end

desc "Run all the tests"
test_runner :tests => [:package_tests] do |tests|
  tests.files = FileList['tests/Chaos.Portal.Mcm.Test.dll']
  tests.add_parameter '/framework=4.0.30319'
  tests.exe = 'tools/NUnit-2.6.0.12051/bin/nunit-console.exe'
end

desc "Merges all production assemblies"
task :package => [:tests] do
  FileUtils.mkdir 'build'

  system 'tools/ILMerge/ILMerge.exe',
    ['/out:build\Chaos.Portal.Mcm.dll',
      '/target:library',
      '/ndebug',
      '/lib:lib',
      '/targetplatform:v4,c:\windows\Microsoft.Net\Framework64\v4.0.30319',
      '/lib:c:\windows\Microsoft.Net\Framework64\v4.0.30319',
      'src\app\CHAOS.MCM\bin\Release\CHAOS.MCM.dll'], clr_command: true
end

task :default => :package
