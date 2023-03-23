﻿/*****************************************************************************
 * Spine Asset Pack License
 * Version 1.0
 * 
 * You are granted a perpetual, non-exclusive, non-sublicensable and
 * non-transferable license to use the Asset Pack and derivative works only as
 * incorporated and embedded components of your software applications and to
 * distribute such software applications. Any source code contained in the Asset
 * Pack may not be distributed in source form. You may otherwise not reproduce,
 * distribute, sublicense, rent, lease or lend the Asset Pack. It is emphasized
 * that you are not entitled to distribute or transfer the Asset Pack in any way
 * other way than as integrated components of your software applications.
 * 
 * THIS ASSET PACK IS PROVIDED BY ESOTERIC SOFTWARE "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL ESOTERIC SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS ASSET PACK, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

//#define IN_CONTROL
using UnityEngine;
using System.Collections;
#if IN_CONTROL
using InControl;
#endif

public class GameSettings : MonoBehaviour {

	public bool slow;
	public UnityEngine.UI.Toggle slowToggle;

	public Animator wipeAnimator;

	void Start () {
		Application.targetFrameRate = 144;
	}

	public void SlowValueChanged () {
		slow = slowToggle.isOn;
	}

	public void ResetLevel () {
		StartCoroutine("ResetLevelRoutine");
	}

	IEnumerator ResetLevelRoutine () {
		wipeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		Application.LoadLevel(Application.loadedLevel);
	}

	void Update () {
#if IN_CONTROL
		if (InputManager.ActiveDevice.MenuWasPressed) {
			slow = !slow;
			slowToggle.isOn = slow;
		}
#endif

		if (Input.GetKeyUp(KeyCode.M)) {
			slow = !slow;
			slowToggle.isOn = slow;
		}

		if (slow) {
			if (Time.timeScale > 0.5f)
				Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.5f, Time.unscaledDeltaTime * 4);
		} else {
			if (Time.timeScale < 2)
				Time.timeScale = Mathf.MoveTowards(Time.timeScale, 2, Time.unscaledDeltaTime * 4);
		}

		if (Input.GetKeyUp(KeyCode.R))
			ResetLevel();
	}
}
