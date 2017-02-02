param($installPath, $toolsPath, $package, $project)
    $project.ProjectItems.Item("SheepAspect.config").Properties.Item("CopyToOutputDirectory").Value = 1
    
	# This is the MSBuild targets file to add
    $targetsFile = [System.IO.Path]::Combine($toolsPath, 'SheepAspect.targets')

	# Add SheepAspect compiler location in the MSBuild targets
	# $targetXml = [Microsoft.Build.Construction.ProjectRootElement]::Open($targetsFile)
	# $targetXml.AddPropertyGroup().addProperty('SheepAspectLocation', $toolsPath)
	# $targetXml.Save()

    # Need to load MSBuild assembly if it's not loaded yet and grab the loaded MSBuild project for the project.
    Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
    # Grab the loaded MSBuild project for the project
    $msbuild = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1

    # Make the path to the targets file relative.
    $projectUri = new-object Uri('file://' + $project.FullName)
    $targetUri = new-object Uri('file://' + $targetsFile)
    $relativePath = $projectUri.MakeRelativeUri($targetUri).ToString().Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)
	

	# Add the import and save the project
	$im = $msbuild.Xml.CreateImportElement($relativePath)
	$im.Condition = "Exists('$relativePath')"
    $msbuild.Xml.InsertAfterChild($im,  ($msbuild.Xml.Imports | Select-Object -Last 1))
    
	$project.Save()