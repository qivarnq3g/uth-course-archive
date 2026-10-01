# fcfs.py

class fcfs:
    def __init__(self, _processID = str, _arrivalTime = int, _burstTime = int):
        self.processID = _processID
        self.arrivalTime = _arrivalTime
        self.burstTime = _burstTime

        self.completeTime = []
        self.TAT = []
        self.WT = []
        self.avgTAT = 0.0
        self.avgWT = 0.0
        self.results = []
    

    def genProcessFlow(self):
        for i in range(len(self.arrivalTime)):
    
        # Tìm kiếm phần tử bé nhất trong dãy chưa được sắp xếp 
            min_idx = i
            for j in range(i+1, len(self.arrivalTime)):
                if self.arrivalTime[min_idx] > self.arrivalTime[j]:
                    min_idx = j
            
            self.arrivalTime[i], self.arrivalTime[min_idx] = self.arrivalTime[min_idx], self.arrivalTime[i]
            self.processID[i], self.processID[min_idx] = self.processID[min_idx], self.processID[i]
            self.burstTime[i], self.burstTime[min_idx] = self.burstTime[min_idx], self.burstTime[i]
    

    def printProcessFlow(self):
        for i in range(len(self.processID)):
            print(self.processID[i], end = " --> ")
        print("end")
    

    def startScheduling(self):
        time = 0
        for i in range(len(self.processID)):
            time = max(time, self.arrivalTime[i])
            self.completeTime.append(time + self.burstTime[i])
            self.TAT.append(self.completeTime[i] - self.arrivalTime[i])
            self.WT.append(self.TAT[i] - self.burstTime[i])
            time = self.completeTime[i]

            self.results.append({
            "Process": self.processID[i],
            "AT": self.arrivalTime[i],
            "BT": self.burstTime[i],
            "CT": self.completeTime[i],
            "TAT": self.TAT[i],
            "WT": self.WT[i]
        })
    

    def calcuAvg(self):
        self.calcuAvgTAT()
        self.calcuAvgWT()


    def calcuAvgTAT(self):
        totalTAT = 0
        for tatVal in self.TAT:
            totalTAT += tatVal
    
        if len(self.processID) > 0:
            self.avgTAT = totalTAT / len(self.processID)
        else:
            self.avgTAT = 0


    def calcuAvgWT(self):
        totalWT = 0
        for wtVal in self.WT:
            totalWT += wtVal

        if len(self.processID) > 0:
            self.avgWT = totalWT / len(self.processID)
        else:
            self.avgWT = 0


    def printResults(self):
        print("--- FCFS scheduling results ---")
        print("{:<10} {:<5} {:<5} {:<5} {:<5} {:<5}".format("Process", "AT", "BT", "CT", "TAT", "WT"))
        for p in self.results:
            print("{:<10} {:<5} {:<5} {:<5} {:<5} {:<5}".format(
                p["Process"], p["AT"], p["BT"], p["CT"], p["TAT"], p["WT"]
        ))

        print("\nAverage Turnaround Time (TAT): " + str(self.avgTAT))
        print("Average Waiting Time (WT): " + str(self.avgWT))