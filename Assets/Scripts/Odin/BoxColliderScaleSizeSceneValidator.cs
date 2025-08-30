#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;
using UnityEditor;

[assembly: RegisterValidationRule(typeof(BoxColliderScaleSizeSceneValidator))]

public class BoxColliderScaleSizeSceneValidator : SceneValidator
{
    // Introduce serialized fields here to make your validator
    // configurable from the validator window under rules.
    public string SpawnerManagerPath = "Managers/Spawner";

    protected override void Validate(ValidationResult result)
    {
        // Scene Validators have many useful methods for querying the scene:

        List<BoxCollider> boxColliders = this.FindAllComponentsInSceneOfType<BoxCollider>().ToList();

        for (int i = 0; i < boxColliders.Count; i++)
        {
            if (boxColliders[i].size.x < 0.0f || boxColliders[i].size.y < 0.0f || boxColliders[i].size.z < 0.0f)
            {
                result.AddError("BoxCollider size is negative");
            }

            if (boxColliders[i].transform.localScale.x < 0.0f || boxColliders[i].transform.localScale.y < 0.0f ||
                boxColliders[i].transform.localScale.z < 0.0f)
            {
                result.AddError("BoxCollider localScale is negative");
            }
        }
        
        // var theScene = this.ValidatedScene;
        // var camera = this.FindComponentInSceneOfType<Camera>();
        // var cameras = this.FindAllComponentsInSceneOfType<Camera>();
        // var allGameObjects = this.GetAllGameObjectsInScene();
        // var rootGameObjects = this.GetSceneRoots();
        // var gameObjectAtPath = this.GetGameObjectAtPath(SpawnerManagerPath);
        // 
        // if (scene has something wrong)
        // {
        //     result.AddError("Something is wrong")
        //         .SetSelectionObject(objectInScene); // Optional
        // }
    }
}
#endif