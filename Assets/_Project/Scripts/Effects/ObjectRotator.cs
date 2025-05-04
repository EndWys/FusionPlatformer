using Unity.Burst;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

namespace Assets._Project.Scripts.Effects
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private Transform[] _objectsToRotate;
        [SerializeField] bool _randomizeStart = true;
        [SerializeField] private float _speed = 90f;

        private TransformAccessArray transformAccessArray;

        private void Awake()
        {
            CollectRotatingObjects();
        }

        public void CollectRotatingObjects()
        {
            var rotatingObjects = FindObjectsOfType<RotatingObject>();
            _objectsToRotate = new Transform[rotatingObjects.Length];

            for (int i = 0; i < rotatingObjects.Length; i++)
            {
                Transform t = rotatingObjects[i].transform;
                _objectsToRotate[i] = t;

                float randomY = Random.Range(0f, 360f);
                Vector3 currentEuler = t.eulerAngles;
                t.eulerAngles = new Vector3(currentEuler.x, randomY, currentEuler.z);
            }

            if (transformAccessArray.isCreated)
                transformAccessArray.Dispose();

            transformAccessArray = new TransformAccessArray(_objectsToRotate);
        }

        private void Update()
        {
            JobHandle handle = RotateJobTask(transformAccessArray);
            handle.Complete();
        }

        private JobHandle RotateJobTask(TransformAccessArray transfromArray)
        {
            var job = new RotateJob
            {
                deltaTime = Time.deltaTime,
                rotationAxis = Vector3.up,
                rotationSpeed = _speed
            };

            JobHandle handle = job.Schedule(transfromArray);
            return handle;
        }
    }

    [BurstCompile]
    public struct RotateJob : IJobParallelForTransform
    {
        public float deltaTime;
        public Vector3 rotationAxis;
        public float rotationSpeed;

        public void Execute(int index, TransformAccess transform)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * deltaTime, rotationAxis.normalized);
            transform.rotation = rotation * transform.rotation;
        }
    }
}