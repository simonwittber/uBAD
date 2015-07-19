# uBAD
Behaviour and Decision Library for Unity

# Example syntax
    Root {
	
        Jump JustLikeAFunction
        Once {
            BB set time 0.5 
        }
        SubTree JustLikeAFunction {
            Sequence {
                Log "1"
                Log "2"
            }
        }
        MutatingSelector {
            While BADTester.CheckSomeCondition {
                Sequence {
                    UntilSuccess {
                        ! BadTester.DoSomeLongTask
                    }
                    Chance 0.5 {
                        Log "Booya."
                    }
                    Chance 0.5 {
                        Sequence {
                            BreakPoint
                            Jump JUMPTOME
                        }
                    }
                }
            
            }

            Label JUMPTOME {
                Sequence {
                    Sleep time, 0.5
                    ? BADTester.CheckSomeCondition
                    ! BADTester.DoSomeLongTask
                    RandomSelector {
                        Sleep time, 0
                        ? BADTester.CheckSomeCondition
                        Cooldown 3 {
                            Invert { 
                                UntilFailure {
                                    ! BADTester.DoSomeLongTask
                                }
                            }
                        }
                    }
                    WaitFor BADTester.CheckSomeCondition {
                        Loop 3 {
                            ! BADTester.DoSomeLongTask
                        }
                    }
                }
            }
        }
        
    }
    
    


