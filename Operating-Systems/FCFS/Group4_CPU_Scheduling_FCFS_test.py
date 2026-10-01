from Group4_CPU_Scheduling_FCFS_class import fcfs

pID0 = ["P00", "P01", "P02", "P03", "P04", "P05"]
arrT0 = [0, 2, 3, 9, 2, 3]
burT0 = [1, 3, 2, 4, 2, 1]

work1 = fcfs(pID0, arrT0, burT0)
work1.genProcessFlow()
work1.startScheduling()
work1.calcuAvg()
work1.printResults()


# Convoy Effect
pID1 = ["P00", "P01", "P02", "P03", "P04", "P05"]
arrT1 = [0, 3, 5, 9, 2, 5]
burT1 = [1, 3, 2, 1, 20, 1]

work2 = fcfs(pID1, arrT1, burT1)
work2.genProcessFlow()
work2.startScheduling()
work2.calcuAvg()
work2.printResults()