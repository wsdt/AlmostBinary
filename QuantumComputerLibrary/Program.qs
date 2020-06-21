namespace Quantum.QuantumComputerLibrary {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Measurement;
    

    operation Set(desired : Result, q1 : Qubit) : Unit {
        if (desired != M(q1)) {
            X(q1);
        }
    }

    operation RandomBitGenerator(): Result {
		using (q = Qubit()) {
			H(q);
            return MResetZ(q);
		}
	}

    operation RandomNumberGenerator(max: Int): Int {
		mutable bits = new Result[0];
        for (idxBit in 1..BitSizeI(max)) {
			set bits += [RandomBitGenerator()];  
		}
        let sample = ResultArrayAsInt(bits);
        return sample > max 
            ? RandomNumberGenerator(max) 
            | sample;
	}

    operation Entanglement(count: Int, initial: Result) : (Int, Int, Int) {
         mutable numOnes = 0;
        mutable agree = 0;
        using ((q0, q1) = (Qubit(), Qubit())) {
            for (test in 1..count) {
                Set(initial, q0);
                Set(Zero, q1);

                H(q0);
                CNOT(q0, q1);
                let res = M(q0);

                if (M(q1) == res) {
                    set agree += 1;
                }

                // Count the number of ones we saw:
                if (res == One) {
                    set numOnes += 1;
                }
            }
            
            Set(Zero, q0);
            Set(Zero, q1);
        }

        // Return number of times we saw a |0> and number of times we saw a |1>
        return (count-numOnes, numOnes, agree);
	}
    
   @EntryPoint()
    operation SampleRandomNumber() : Int {
        let max = 50;
        Message($"Sampling a random number between 0 and {max}: ");
        return RandomNumberGenerator(max);
    }
}
