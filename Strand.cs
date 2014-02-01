using UnityEngine;
using System.Collections;

public class Strand : MonoBehaviour
{
    private const int numStrandParticles = 10;
    private const int numSprings = numStrandParticles - 1;

    private StrandParticle[] StrandParticles = new StrandParticle[numStrandParticles];
    private Spring[] springs = new Spring[numSprings];

    private Vector3[] forces = new Vector3[numStrandParticles];

    private GameObject[] hairGameObjects = new GameObject[numStrandParticles];

    private const float damping = -0.999f;
    private const float stifness = 8f;
    private const float restLength = 0.035f;
    private const float drag = 0.001f;

    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        InitializeStrandParticles();
        InitializeGameObjects();
        InitializeSprings();
        InitLineRenderer();
    }

    private void InitLineRenderer()
    {
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetWidth(0.05f, 0.05f);
        lineRenderer.SetVertexCount(numStrandParticles);
    }

    private void InitializeSprings()
    {
        for (int i = 0; i < numSprings; i++)
        {
            springs[i] = new Spring(i, i + 1, restLength, stifness - i * 0.5f, damping);
        }
    }

    private void InitializeGameObjects()
    {
        float scale = 0.1f;
        for (int i = 0; i < numStrandParticles; i++)
        {
            hairGameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hairGameObjects[i].transform.position = StrandParticles[i].Position;
            hairGameObjects[i].transform.localScale = new Vector3(scale, scale, scale);
            hairGameObjects[i].name = "particle: " + i.ToString();
        }
    }

    private void InitializeStrandParticles()
    {
        float rootMass = 0.05f;
        float massToRemove = 0.0015f;
        Vector3 startPos = new Vector3(0f, 5f, 0f);
        for (int i = numStrandParticles - 1; i >= 0; i--)
        {
            float mass = rootMass - i * massToRemove;
            StrandParticles[i] = new StrandParticle(mass, new Vector3(0, startPos.y - 0.3f * i, 0));
        }
    }

    private void ComputeDefaultForces()
    {
        for (int i = 0; i < numStrandParticles; i++)
        {
            forces[i] = Vector3.zero;
            Vector3 verletVelocity = VerletUtil.GetVerletVelocity(StrandParticles[i].Position, StrandParticles[i].LastPosition, Time.deltaTime);
            if (i != 0)
            {
                forces[i] = VerletUtil.AddGravityForce(forces[i], StrandParticles[i].Mass);
                forces[i] = VerletUtil.AddDampingForce(forces[i], verletVelocity);
                //forces[i] = VerletUtil.AddDragForce(forces[i], verletVelocity, drag);
            }
        }
    }

    private void ComputeSpringForcesSingle()
    {
        for (int i = 0; i < numSprings; i++)
        {
            Vector3 p1 = StrandParticles[i].Position;
            Vector3 p1Last = StrandParticles[i].LastPosition;

            Vector3 p2 = StrandParticles[i + 1].Position;
            Vector2 p2Last = StrandParticles[i + 1].LastPosition;

            Vector3 deltaPos = p1 - p2;
            Vector3 deltaVel = VerletUtil.GetDeltaVerletVelocity(p1, p1Last, p2, p2Last, Time.deltaTime);

            float distance = Vector3.Magnitude(deltaPos);

            float t1 = -springs[i].Ks * (distance - springs[i].RestLength);
            float t2 = springs[i].Kd * (Vector3.Dot(deltaVel, deltaPos) / distance);

            Vector3 springForce = (t1 + t2) * Vector3.Normalize(deltaPos);
            if (springs[i].P1 != 0) forces[springs[i].P1] += springForce;
            if (springs[i].P2 != 0) forces[springs[i].P2] -= springForce;
        }
    }

    void IntegrateVerlet()
    {
        for (int i = 0; i < numStrandParticles; i++)
        {
            Vector3 buffer = StrandParticles[i].Position;
            float ddm = (Time.deltaTime * Time.deltaTime) / StrandParticles[i].Mass;
            StrandParticles[i].Position += (StrandParticles[i].Position - StrandParticles[i].LastPosition) + ddm * forces[i];
            StrandParticles[i].LastPosition = buffer;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Manipulate();
        }
        ComputeDefaultForces();
        ComputeSpringForcesSingle();
        IntegrateVerlet();

        for (int i = 0; i < numStrandParticles; i++)
        {
            hairGameObjects[i].transform.position = StrandParticles[i].Position;
        }
        RenderLines();
    }

    private void RenderLines()
    {
        for (int i = 0; i < numStrandParticles; i++)
        {
            lineRenderer.SetPosition(i, StrandParticles[i].Position);
        }

    }

    private void Manipulate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 newPos = new Vector3(ray.origin.x + ray.direction.x * 10, ray.origin.y + ray.direction.y * 10, 0f);
        StrandParticles[0].Position = newPos;
        StrandParticles[0].Position = newPos;
    }
}
