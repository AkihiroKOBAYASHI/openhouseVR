using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (MyCarController))]
	public class MyCarUserControl : MonoBehaviour
	{

		[SerializeField] private bool isKeyboardMode = false;

		private MyCarController m_Car;
		private bool push = true;
		private float delay = 0.2f;

		private void Awake()
		{
			// get the car controller
			m_Car = GetComponent<MyCarController>();
		}

		private void FixedUpdate() {
			// LR
			float h;
			if (isKeyboardMode) {
				h = CrossPlatformInputManager.GetAxis ("Horizontal");
			} else {
				h = Input.GetAxis("Handle");
			}

			// Straight
			float v;
			if (isKeyboardMode) {
				v = CrossPlatformInputManager.GetAxis ("Vertical");
			} else {
				v = Input.GetAxis("Accel") * (-1f);
				v = (v  + 1f) * 0.5f;
			}

			// Brake
			float s;
			if (isKeyboardMode) {
				s = CrossPlatformInputManager.GetAxis("Space");
			} else {
				s = Input.GetAxis("Brake") * (-1f);
				s = (s  + 1f) * 0.5f;
			}

			// Back = backtrigger + straight
			float b = Input.GetAxis("BackTrigger");

			// Decide
			bool d = CrossPlatformInputManager.GetButtonUp("Decide");

			if(s > 0) {
				m_Car.Move(h, 0, 0, s); // Stop
			}
			else {
				if (v == 0) {
					m_Car.Move (h, 0, 0, 0); // Do nothing
				} else {
					if (b == 0) {
						m_Car.Move (h, v, 0, 0); // Go
					} else {
						m_Car.Move (h, 0, (-1) * v, 0); // Back
					}
				}
			}

			if (d && push) {
				push = false;
				GameController.instance.ChangeGameScene ();
				StartCoroutine(PreventSuccessionPush ());
			}
		}

		private IEnumerator PreventSuccessionPush() {
			yield return new WaitForSeconds(delay);
			push = true;
		}
	}
}
