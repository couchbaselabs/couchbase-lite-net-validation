#!/bin/bash

pushd `dirname $0`
sed "s/[0-9].[0-9].[0-9]-b..../$1-$2/g" Couchbase.Lite.Tests.Maui.csproj > tmp
mv tmp Couchbase.Lite.Tests.Maui.csproj
popd
