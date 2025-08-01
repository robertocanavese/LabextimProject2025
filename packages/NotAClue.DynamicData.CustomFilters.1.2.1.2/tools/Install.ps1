# add Link for AutocompleteStyle.css
Get-ChildItem . -Recurse -Include Site.master | Foreach-Object { (Get-Content $_) | Foreach-Object { $_ -replace "</head>", "    <link href=`"AutocompleteStyle.css`" rel=`"stylesheet`" type=`"text/css`" />`r`n</head>" } | Set-Content $_ -Force }
