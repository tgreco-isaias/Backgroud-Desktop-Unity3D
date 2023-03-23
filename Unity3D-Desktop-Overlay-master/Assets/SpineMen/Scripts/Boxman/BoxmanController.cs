/*****************************************************************************
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
using Spine.Unity.Examples;

public class BoxmanController : GameCharacter {
	public enum ActionState { IDLE, WALK, DANCE, DEAD }

	[Header("Animations")]
	[SpineAnimation]
	public string idleAnim;
	[SpineAnimation]
	public string walkAnim;
	[SpineAnimation]
	public string danceAnim;
	[SpineAnimation]
	public string hitForwardAnim;
	[SpineAnimation]
	public string hitBackwardAnim;

	[Header("Stats")]
	public float hp = 4;

	[Header("Audio")]
	public string hitSound;

	[Header("Physics")]
	public Collider2D primaryCollider;
	public LayerMask platformMask;

	[Header("References")]
	public SkeletonAnimation skeletonAnimation;
	public GameObject punchHitPrefab;

	public ActionState state;

	SkeletonUtility skeletonUtility;
	Rigidbody2D rb;
	bool flipped = false;

	void Start () {
		

		rb = GetComponent<Rigidbody2D>();

		if (Random.value > 0.5f) {
			skeletonAnimation.skeleton.FlipX = !skeletonAnimation.skeleton.FlipX;
			flipped = skeletonAnimation.skeleton.FlipX;
		}
	}

	void OnEnable () {
		Register();
	}

	void OnDisable () {
		Unregister();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		var boundingBoxFollower = collider.GetComponent<BoundingBoxFollower>();
		if (boundingBoxFollower != null) {
			var attachmentName = boundingBoxFollower.CurrentAttachmentName;

			int fromSign = collider.transform.position.x < transform.position.x ? -1 : 1;
			
			switch (attachmentName) {
				case "Punch":
					Hit(new Vector2(-fromSign * 2, 0), 1, fromSign);
					PunchImpact(collider, Vector2.zero, new Vector2(-fromSign, 0));
					break;
				case "UpperCut":
					Hit(new Vector2(-fromSign * 4, 8), 4, fromSign);
					PunchImpact(collider, new Vector2(0,1.25f), new Vector2(0, 1));
					break;
				case "HeadDive":
					Hit(new Vector2(0, -8), 8, 0);
					break;
			}
		}
	}

	void PunchImpact (Collider2D collider, Vector2 offset, Vector2 direction) {
		Instantiate(punchHitPrefab, collider.bounds.center + (Vector3)offset, Quaternion.FromToRotation(Vector3.right, direction));
	}

	void Hit (float damage) {
		Hit((hp - damage <= 0) ? new Vector2(0, -12) : Vector2.zero, damage, 0);
	}

	void Hit (HitData data) {
		Hit(data.velocity, data.damage, data.origin.x > data.point.x ? 1 : -1);
	}

	void Hit (Vector2 velocity, float damage, int fromSign) {

		SoundPalette.PlaySound(hitSound, 1, 1, transform.position);

		if (hp <= 0)
			return;

		hp -= damage;

		if (hp <= 0) {
			state = ActionState.DEAD;
			var ragdoll = GetComponentInChildren<SkeletonRagdoll2D>();
			ragdoll.Apply();
			ragdoll.RootRigidbody.velocity = velocity * 10;
			var agent = ragdoll.RootRigidbody.gameObject.AddComponent<MovingPlatformAgent>();
			var rootCollider = ragdoll.RootRigidbody.GetComponent<Collider2D>();
			agent.platformMask = platformMask;
			agent.castRadius = rootCollider.GetType() == typeof(CircleCollider2D) ? ((CircleCollider2D)rootCollider).radius * 8f : rootCollider.bounds.size.y;
			agent.useCircleMode = true;
			var rbs = ragdoll.RootRigidbody.transform.parent.GetComponentsInChildren<Rigidbody2D>();
			foreach (var r in rbs) {
				r.gameObject.AddComponent<RagdollImpactEffector>();
			}

			Destroy(rb);
			Destroy(primaryCollider);
		} else {
			rb.velocity = velocity;

			string anim = "";
			if ((flipped ? -1 : 1) != fromSign) {
				anim = hitBackwardAnim;
			} else {
				anim = hitForwardAnim;
			}

			skeletonAnimation.state.SetAnimation(0, anim, false);
			skeletonAnimation.state.AddAnimation(0, idleAnim, true, 0.2f);
		}
	}

	void FixedUpdate () {
		if (rb == null)
			return;

		var movingPlatform = MovingPlatformCast(new Vector3(0, 0.1f, 0));

		if (movingPlatform) {
			var track = skeletonAnimation.state.GetCurrent(0);

			//hacky, but functional.  only apply moving velocity if not being hit.
			if (track.Animation.Name == idleAnim) {
				Vector3 velocity = rb.velocity;
				velocity.x = movingPlatform.Velocity.x;
				velocity.y = movingPlatform.Velocity.y;
				rb.velocity = velocity;
			}
			
		}
	}

	MovingPlatform MovingPlatformCast (Vector3 origin) {
		MovingPlatform platform = null;
		RaycastHit2D hit = Physics2D.Raycast(transform.TransformPoint(origin), -Vector2.up, 0.2f, platformMask);
		if (hit.collider != null) {
			platform = hit.collider.GetComponent<MovingPlatform>();
		}


		return platform;
	}
}
