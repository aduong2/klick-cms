#!/usr/bin/env sh
#Get namespace
echo
userinp=
echo -n "Namespace (e.g. Client.ProjectName): "
read userinp
if test "$userinp" = ""; then exit; fi
namespace=$userinp

#Set projectname
echo
project=${namespace##*.}


# Get the remote git repo
gitrepo=
echo -n "The remote git repo to push this project to (enter for none): "
read userinp
if test "$userinp" != ""; then gitrepo=$userinp; fi
echo 

#UPDATE NAMESPACES
echo "Setting Namespace: $namespace"
find . -type f | grep -v .git | grep -v init.sh | xargs perl -p -i.bak -e s/Klick.BoilerPlate/$namespace/g
echo

#UPDATE FOLDER
echo "Changing folder and project name"
mv Web/Klick.BoilerPlate.csproj Web/$namespace.csproj
mv Tests/Klick.BoilerPlate.Tests.csproj Tests/$namespace.Tests.csproj
mv Klick.BoilerPlate.sln $namespace.sln

# remove bak 
echo
echo "Removing *.bak Files" 
find . -type f -name '*.bak' |xargs rm

# remove and .git files copied from Boilerplate and initialize a new git repo
rm -rf .git 

echo "Initializing new git repo"
git init
git add *
git commit -m "Initial site"

# remove the init.sh file from the newly created repo

echo "Remove init.sh from the initial repo"
git rm --cached init.sh

if test "$gitrepo" != ""; then 
	git remote add origin $gitrepo
	git push -u origin master
fi
