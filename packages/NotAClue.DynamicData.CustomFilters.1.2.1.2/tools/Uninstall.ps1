# remove Link for AutocompleteStyle.css
Get-ChildItem . -Recurse -Include Site.master | Foreach-Object { (Get-Content $_) | Foreach-Object { $_ -replace "<link href=`"AutocompleteStyle.css`" rel=`"stylesheet`" type=`"text/css`" />", "" } | Set-Content $_ -Force }
