using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS32_Sim_Core {
    //test

    /// <summary>
    /// Class to represent an "instance" of a MIPS32 chip that operations are performed on
    /// </summary>
    public class MIPSChip_Instance {
        private UInt32[] registers;
        private UInt32 hi;
        private UInt32 lo;

        public UInt32 HI {
            get { return this.hi; }
            set { this.hi = value; }
        }
        public UInt32 LO {
            get { return this.lo; }
            set { this.lo = value; }
        }

        /// <summary>
        /// Returns the value contained in the specified register
        /// </summary>
        /// <param name="reg">Which register to grab the value from</param>
        /// <returns></returns>
        public UInt32 getRegister(int reg) {
            return registers[reg];
        }

        /// <summary>
        /// Forces the specified register to have the specified value
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="value"></param>
        internal void setRegister(int reg, UInt32 value) {
            this.registers[reg] = value;
        }

        /// <summary>
        /// signal an interrupt TODO
        /// </summary>
        internal void interrupt() {
            //TODO
        }
    }

    /// <summary>
    /// Describes any MIPS operation that can be performed on MIPSChip_Instance
    /// </summary>
    public interface IOperation {
        void doOp(MIPSChip_Instance chip);
        void undoOp(MIPSChip_Instance chip);
        UInt32 getByteCode();
        //String getName();
        //String getFormat();
        //String getDesc();
    }

}
