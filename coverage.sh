echo
echo ">>> Running test..."
echo

export SolutionFile=NgWebApp.sln
export CollectCoverage=true
export CoverletOutput=../CodeCoverage/
export CoverletOutputFormat=opencover

dotnet test $SolutionFile --no-build $CollectCoverage
echo ">>> Build coverage report..."
echo
dotnet /Users/"$USER"/.nuget/packages/reportgenerator/5.1.17/tools/net7.0/ReportGenerator.dll "-reports:CodeCoverage/coverage.opencover.xml" "-targetdir:CodeCoverage/Web" "-assemblyfilters:-Web;-Web.Views;" "-classfilters:-*Exception"

echo
open CodeCoverage/Web/index.html
