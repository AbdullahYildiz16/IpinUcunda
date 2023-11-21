using UnityEngine;

namespace _Scripts
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private ParticleSystem poofParticlePrefab;
        [SerializeField] private ParticleSystem blastParticlePrefab;

        public void PlayPoofParticle(Vector3 pos)
        {
            var ps = Instantiate(poofParticlePrefab, pos, Quaternion.identity);
            ps.transform.position = pos;
            ps.Play();
            Destroy(ps.gameObject, 1f);
        }

        public void PlayBlastParticle(Vector3 pos)
        {
            var ps = Instantiate(blastParticlePrefab, pos, Quaternion.identity);
            ps.transform.position = pos;
            ps.Play();
            Destroy(ps.gameObject, 1f);
        }
    }
}