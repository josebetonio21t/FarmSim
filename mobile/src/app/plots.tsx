import { useRouter } from 'expo-router';
import { useState } from 'react';
import { FlatList, StyleSheet, Text, TouchableOpacity, View } from 'react-native';

interface PlotItem {
  id: string;
  name: string;
  type: string;
  progress: number;
  totalDays: number;
  health: string;
}

const PLOT_DATA: PlotItem[] = [
  { id: 'A', name: 'Cabbage (Kimchi)', type: 'VEGE', progress: 11, totalDays: 21, health: '90%' },
  { id: 'B', name: 'Cabbage (vege)', type: 'VEGE', progress: 11, totalDays: 21, health: '90%' },
  { id: 'C', name: 'Cabbage (Kimchi)', type: 'VEGE', progress: 11, totalDays: 21, health: '90%' },
  { id: 'D', name: 'Cabbage (Kimchi)', type: 'VEGE', progress: 11, totalDays: 21, health: '90%' },
];

export default function PlotsScreen() {
  const router = useRouter();
  const [selectedPlot, setSelectedPlot] = useState<string>('A'); 

  const renderPlotCard = ({ item }: { item: PlotItem }) => {
    const isSelected = item.id === selectedPlot;
    const progressPercent = (item.progress / item.totalDays) * 100;

    return (
      <TouchableOpacity
        activeOpacity={0.8}
        onPress={() => {
          setSelectedPlot(item.id);
          router.push(`/plot/${item.id}` as any); 
        }}
        style={[styles.card, isSelected && styles.selectedCard]}
      >
        <View style={styles.cardHeader}>
          <Text style={styles.plotLabel}>PLOT {item.id}</Text>
          <View style={[styles.badge, isSelected && styles.selectedBadge]}>
            <Text style={[styles.badgeText, isSelected && styles.selectedBadgeText]}>{item.type}</Text>
          </View>
        </View>

        <Text style={styles.cropName}>{item.name}</Text>

        <View style={styles.progressBarBg}>
          <View style={[styles.progressBarFill, { width: `${progressPercent}%` }]} />
        </View>

        <View style={styles.cardFooter}>
          <Text style={styles.footerTextMuted}>Day {item.progress}/{item.totalDays}</Text>
          <Text style={styles.healthText}>{item.health} Health</Text>
        </View>
      </TouchableOpacity>
    );
  };

  return (
    <View style={styles.container}>
      <Text style={styles.sectionHeader}>GROW PLOTS</Text>
      <FlatList
        data={PLOT_DATA}
        renderItem={renderPlotCard}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.listContainer}
        showsVerticalScrollIndicator={false}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#111411', paddingHorizontal: 16, paddingTop: 16 },
  sectionHeader: { color: '#455545', fontSize: 14, fontWeight: 'bold', letterSpacing: 1, marginBottom: 16 },
  listContainer: { gap: 16, paddingBottom: 24 },
  card: { backgroundColor: '#151915', borderRadius: 8, borderWidth: 1, borderColor: '#222c22', padding: 16 },
  selectedCard: { borderColor: '#00E639', shadowColor: '#00E639', shadowOffset: { width: 0, height: 0 }, shadowOpacity: 0.15, shadowRadius: 8, elevation: 3 },
  cardHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  plotLabel: { color: '#455545', fontSize: 11, fontWeight: '600', letterSpacing: 0.5 },
  badge: { backgroundColor: '#1a221a', paddingHorizontal: 10, paddingVertical: 4, borderRadius: 4 },
  selectedBadge: { backgroundColor: '#0c3a14' },
  badgeText: { color: '#455545', fontSize: 10, fontWeight: 'bold' },
  selectedBadgeText: { color: '#00E639' },
  cropName: { color: '#E0E7E0', fontSize: 18, fontWeight: '500', marginBottom: 12 },
  progressBarBg: { height: 3, backgroundColor: '#1e261e', borderRadius: 2, marginBottom: 10, overflow: 'hidden' },
  progressBarFill: { height: '100%', backgroundColor: '#00E639', borderRadius: 2 },
  cardFooter: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  footerTextMuted: { color: '#455545', fontSize: 12 },
  healthText: { color: '#00E639', fontSize: 12, fontWeight: '600' },
});
