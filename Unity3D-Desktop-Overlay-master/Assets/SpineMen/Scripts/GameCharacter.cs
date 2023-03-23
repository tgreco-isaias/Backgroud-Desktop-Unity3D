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
using System.Collections.Generic;

/// <summary>
/// This class keeps track of all "Characters"
/// IE: This can be used to intentionally ignore collision against specific characters rather than entire layers.
/// </summary>
public class GameCharacter : MonoBehaviour {
	public static List<GameCharacter> All = new List<GameCharacter>();

	public virtual void Register () {
		All.Add(this);
	}

	public virtual void Unregister () {
		All.Remove(this);
	}

	public virtual void IgnoreCollision (Collider2D collider, bool ignore) {
		Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), collider, ignore);
	}
}
