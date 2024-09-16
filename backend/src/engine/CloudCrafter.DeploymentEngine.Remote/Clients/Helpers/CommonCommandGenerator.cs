﻿using System.Text;
using CloudCrafter.DeploymentEngine.Remote.Clients.Contracts;

namespace CloudCrafter.DeploymentEngine.Remote.Clients.Helpers;

public class CommonCommandGenerator : ICommonCommandGenerator
{
    public string WriteContentsToFile(string contents, string filePath)
    {
        var base64Contents = Convert.ToBase64String(Encoding.UTF8.GetBytes(contents));
        return $"echo '{base64Contents}' | base64 -d > {filePath}";
    }

    public string PullDockerImage(string image)
    {
        return $"docker pull {image}";
    }

    public string VerifyDockerImageExists(string image)
    {
        return $"docker image inspect {image} --format=\"{{{{.Id}}}}\"";
    }

    public string CreateHelperContainer(Guid deploymentId, string image)
    {
        return $"docker run -d --rm -v /var/run/docker.sock:/var/run/docker.sock --name deployment-{deploymentId} {image}";
    }

    public string GenerateRandomFile()
    {
        return "mktemp";
    }

    public string RunRecipe(string recipeFile)
    {
        return $"/usr/local/bin/cloudcrafter-agent --recipe {recipeFile}";
    }

    public string RunInDockerContainer(string containerId, string command)
    {
        return $"docker exec {containerId} bash -c '{command}'";
    }
}
