using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TrajectoryHelper {
	
	public static void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity, LineRenderer trajectoryRenderer, int steps)
	{
		float timeDelta = 1.0f / initialVelocity.magnitude; 

		trajectoryRenderer.enabled = true;
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < steps; ++i)
		{
			trajectoryRenderer.SetPosition(i, position);

			position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
			velocity += gravity * timeDelta;
		}

		trajectoryRenderer.SetVertexCount(steps);
	
	}


	public static void resetTrajectory(LineRenderer trajectoryRenderer, int steps) {
		for (int i = 0; i < steps; ++i)
		{
			trajectoryRenderer.SetPosition(i, Vector3.zero);

		}
		trajectoryRenderer.SetVertexCount(0);



	}
}
