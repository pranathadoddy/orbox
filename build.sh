docker login registry.gitlab.com
docker build --no-cache -f ./Orderbox.Mvc/Dockerfile -t registry.gitlab.com/iketut/obox .
docker push registry.gitlab.com/iketut/obox
docker logout