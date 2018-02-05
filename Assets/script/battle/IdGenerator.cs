using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle {
    public static class IdGenerator {

        public static int GeneratorEffectId() {
            if (m_EffectId >= 0)
                return ++m_EffectId;
            m_EffectId = 0;
            return m_EffectId;
        }

        private static int m_EffectId = -1;
    }
}
