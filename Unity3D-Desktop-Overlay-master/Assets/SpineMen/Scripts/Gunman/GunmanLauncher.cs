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
using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class GunmanLauncher : GunmanWeapon {

	public GameObject missilePrefab;
	public Transform muzzle;
	public LayerMask targetMask;
	public float recoil = 4;

	[SpineSlot]
	public string missileSlot;
	public SkeletonRenderer weaponRenderer;

    Spine.Slot slot;

	public override void Setup () {
        slot = weaponRenderer.skeleton.FindSlot(missileSlot);

        if (clip == 0)
			slot.Attachment = null;
	}

    private void Update()
    {

        
        if (clip == 0)
        {
            var entry = ((SkeletonAnimation)weaponRenderer).AnimationState.GetCurrent(2);
            if (entry != null && entry.Animation == ReloadAnim)
            {
                //do nothing
            }
            else
            {
                slot.Attachment = null;
            }
        }
            
    }

    public override void Fire () {
		Vector3 position = muzzle.position;
		Vector3 dir = muzzle.right;
		clip--;
		Instantiate(missilePrefab, position, Quaternion.FromToRotation(Vector3.right, dir));
	}

	public override Vector2 GetRecoil () {
		Vector2 r = muzzle.right * this.recoil;
		r.y *= 0.4f;
		return r;
	}
}
